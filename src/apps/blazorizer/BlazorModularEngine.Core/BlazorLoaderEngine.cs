 
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.Loader;
using BlazorModularEngine.Core.Common.Interfaces;
using BlazorModularEngine.Core.Hooks;
using BlazorModularEngine.Core.Models;
using BlazorModularEngine.Core.Services;
using BlazorModularEngine.Core.UI;
using Microsoft.Extensions.DependencyInjection;

public class BlazorLoaderEngine
{
    private readonly HttpClient _httpClient;
    private readonly ModuleCacheStore _cacheStore;
    private readonly IServiceCollection _services;
    private readonly DynamicServiceRegistry _dynamicRegistry;
    private readonly PluginHookManager _hookManager;
    private readonly PluginSlotRegistry _slotRegistry;
    private IServiceProvider _serviceProvider;

    public event Action<string, int> OnLoadingProgressChanged;
    public event Action<bool> OnLoaderVisibilityChanged;

    public BlazorLoaderEngine(
        HttpClient httpClient,
        ModuleCacheStore cacheStore,
        IServiceCollection services,
        DynamicServiceRegistry dynamicRegistry,
        PluginHookManager hookManager,
        PluginSlotRegistry slotRegistry)
    {
        _httpClient = httpClient;
        _cacheStore = cacheStore;
        _services = services;
        _dynamicRegistry = dynamicRegistry;
        _hookManager = hookManager;
        _slotRegistry = slotRegistry;
    }

    public async Task FetchMetadataAsync(string apiEndpoint)
    {
        OnLoadingProgressChanged?.Invoke("Fetching plugin metadata...", 10);
        var metadataList = await _httpClient.GetFromJsonAsync<List<ModuleMetadata>>(apiEndpoint) ?? new List<ModuleMetadata>();
        _cacheStore.StoreMetadata(metadataList);
        OnLoadingProgressChanged?.Invoke("Metadata cached successfully.", 25);
    }

    public async Task<IBlazorModule> EnsureModuleLoadedAsync(string moduleName, HashSet<string> activeChain = null)
    {
        if (_cacheStore.HasInstance(moduleName)) return _cacheStore.GetInstance(moduleName);

        activeChain ??= new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (activeChain.Contains(moduleName)) throw new InvalidOperationException($"Circular dependency detected involving module: '{moduleName}'.");
        activeChain.Add(moduleName);

        var meta = _cacheStore.GetMetadata(moduleName) ?? throw new KeyNotFoundException($"Module metadata for '{moduleName}' not found.");

        foreach (var depName in meta.Dependencies)
        {
            await EnsureModuleLoadedAsync(depName, activeChain);
        }

        OnLoadingProgressChanged?.Invoke($"Downloading plugin: {moduleName}...", 45);
        byte[] assemblyBytes = await _httpClient.GetByteArrayAsync(meta.AssemblyUrl);

        Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(new MemoryStream(assemblyBytes));
        _cacheStore.StoreAssembly(moduleName, assembly);

        OnLoadingProgressChanged?.Invoke($"Initializing plugin: {moduleName}...", 60);
        var moduleInstance = CreateModuleInstance(assembly);

        if (moduleInstance != null)
        {
            bool isRunning = (_serviceProvider != null);
            IServiceCollection targetServices = new RuntimeServiceCollectionAdapter(_services, _dynamicRegistry, isRunning);

            moduleInstance.ConfigureServices(targetServices);

            moduleInstance.RegisterHooks(_hookManager);
            moduleInstance.RegisterUI(_slotRegistry);

            _cacheStore.StoreInstance(moduleName, moduleInstance);

            if (isRunning)
            {
                await moduleInstance.InitializeAsync(_serviceProvider);
            }
        }

        activeChain.Remove(moduleName);
        return moduleInstance;
    }

    public async Task LoadStartupModulesAsync(IProgress<string> progress)
    {
        OnLoaderVisibilityChanged?.Invoke(true);
        var metadataList = _cacheStore.GetAllMetadata();
        int total = 0, current = 0;

        foreach (var meta in metadataList) if (meta.IsStartupModule) total++;

        foreach (var meta in metadataList)
        {
            if (!meta.IsStartupModule) continue;
            progress?.Report($"Loading startup plugin: {meta.ModuleName}...");
            await EnsureModuleLoadedAsync(meta.ModuleName);
            current++;
            int percent = 25 + (int)((current / (float)Math.Max(1, total)) * 50);
            OnLoadingProgressChanged?.Invoke($"Loaded {meta.ModuleName}", percent);
        }
    }

    public async Task<IBlazorModule> LazyLoadModuleAsync(string moduleName)
    {
        if (_cacheStore.HasInstance(moduleName)) return _cacheStore.GetInstance(moduleName);

        OnLoaderVisibilityChanged?.Invoke(true);
        OnLoadingProgressChanged?.Invoke($"Preparing lazy load for {moduleName}...", 40);
        var moduleInstance = await EnsureModuleLoadedAsync(moduleName);
        OnLoaderVisibilityChanged?.Invoke(false);
        return moduleInstance;
    }

    private IBlazorModule CreateModuleInstance(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (typeof(IBlazorModule).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            {
                return Activator.CreateInstance(type) as IBlazorModule;
            }
        }
        return null;
    }

    public async Task FinalizeAndSwapAppAsync(IServiceProvider finalizedServiceProvider)
    {
        _serviceProvider = finalizedServiceProvider;
        foreach (var meta in _cacheStore.GetAllMetadata())
        {
            var instance = _cacheStore.GetInstance(meta.ModuleName);
            if (instance != null) await instance.InitializeAsync(_serviceProvider);
        }
        OnLoadingProgressChanged?.Invoke("Ready!", 100);
        await Task.Delay(300);
        OnLoaderVisibilityChanged?.Invoke(false);
    }
}
