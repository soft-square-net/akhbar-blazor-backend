// Services/PluginManager.cs
using System.Reflection;
using System.Net.Http.Json;
using FSH.Starter.Blazor.Modules.Plugins;
using FSH.Starter.Blazor.Modules.Plugins.Hooks.Base;
using FSH.Starter.BlazorShared;
// using System.Runtime.Loader;
using IApiClient = FSH.Starter.Blazor.Infrastructure.Api.IApiClient;
using SearchPluginsCommand = FSH.Starter.Blazor.Infrastructure.Api.SearchPluginsCommand;
using Mapster;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nextended.Core.Extensions;
using Assembly = System.Reflection.Assembly;
// using PluginModelDto = Shared.FSHPlugin.PluginModelDto;
using Shared.FSHPlugin;

namespace FSH.Starter.Blazor.Modules.Services;
public class PluginManager
{
    // private readonly List<PluginHookBase> _registeredHooks = [];
    // private List<PluginCacheItem> PluginsStore = new();
    // private readonly HttpClient _httpClient;
    private readonly IApiClient _apiClient;
    private readonly ILogger _logger;
    private readonly List<PluginModelDto> _plugins = new();
    // private Assembly mainAssembly = Assembly.GetEntryAssembly();
    private readonly WebAssemblyHostBuilder _builder;
    // public event Action<string>? OnLoadingProgressChanged;
    public event Action? OnPluginsChanged;
    public event Action<Assembly>? OnAssemblyLoaded;
    public IReadOnlyList<PluginModelDto> Plugins => _plugins?.AsReadOnly() ?? new List<PluginModelDto>().AsReadOnly();
    public bool IsInitialized { get; private set; } = false;

    public PluginManager(IApiClient apiClient, WebAssemblyHostBuilder builder, ILogger logger)
    {
        // _httpClient  = builder.Services.BuildServiceProvider().GetRequiredService<HttpClient>(); ;
        _apiClient = apiClient;
        _logger = logger;
        _builder = builder;
    }

    // public void RegisterHook(PluginHookBase hook)
    // {
    //     _registeredHooks.Add(hook);
    // }

    // Generic execution helper for pure function hooks expecting a return type T
    // public T? ExecuteHook<T>(string hookName, Dictionary<string, object?> parameters)
    // {
    //     var hook = _registeredHooks
    //         .OfType<PluginHook<T>>()
    //         .FirstOrDefault(h => h.Name == hookName);
    //
    //     if (hook?.HookFunc is not null)
    //     {
    //         return hook.HookFunc(this, parameters);
    //     }
    //
    //     return default;
    // }
    //
    // public Func<PluginManager, Dictionary<string, object?>, object?> Bootstrap {get; set;}
    /// <summary>
    /// Boot phase: Fetches manifest from API and loads only startup plugins.
    /// </summary>
    public async Task InitializeStartupPluginsAsync(Action<string> reportProgress)
    {
        if (IsInitialized) return;

        try
        {
            reportProgress("Connecting to server API to fetch plugin manifest...");
            await Task.Delay(400); // UI breathing room simulation

            // Fetch list of plugins from your backend API endpoint
            // var manifest = await _httpClient.GetFromJsonAsync<List<PluginModelDto>>("api/plugins");
            var manifest = (await _apiClient.SearchPluginsEndpointAsync(new SearchPluginsCommand())).Items;
            // Get Plugins From ConFigurations Better than above
            if (manifest != null)
            {
                _plugins.Clear();
                // _plugins.AddRange(cvrt);
                _plugins.AddRange(manifest.Select(apiPlugin => new PluginModelDto
                {
                    Id = apiPlugin.Id,
                    Name = apiPlugin.Name,
                    DllUrl = apiPlugin.DllUrl,
                    PluginFileName =  apiPlugin.PluginFileName,
                    AssemblyData =  apiPlugin.AssemblyData,
                    IsStartup = apiPlugin.IsStartup,
                    IsEnabled = apiPlugin.IsEnabled
                    // LoadedAssembly is left null on purpose; it gets populated when downloaded from the DLL bytes
                }));
            }

            // Filter enabled startup plugins
            var startupPlugins = _plugins.Where(p => p.IsEnabled && p.IsStartup).ToList();
            
            foreach (var plugin in startupPlugins)
            {
                reportProgress($"Loading startup module: {plugin.Name}...");
                // await LoadAssemblyBytesAsync(plugin);
                LoadPluginFromApiAsync(plugin,  _builder, _logger);
            }

            IsInitialized = true;
            reportProgress("Initialization complete. Starting application...");
            await Task.Delay(3000);
        }
        catch (Exception ex)
        {
            reportProgress($"Error loading plugins: {ex.Message}");
            throw;
        }
    }
#region Lazy Loading
    /// <summary> ************************************************
    /// Lazy loads a non-startup plugin on demand.
    /// </summary>
    public async Task<bool> LazyLoadPluginAsync(string id)
    {
        var plugin = _plugins.FirstOrDefault(p => p.Id == id);
        if (plugin == null || !plugin.AssemblyData.IsEmpty()) return true;

        if (plugin.IsEnabled)
        {
            await LoadAssemblyBytesAsync(plugin);
            OnPluginsChanged?.Invoke();
            return true;
        }
        return false;
    }

    private async Task LoadAssemblyBytesAsync(PluginModelDto plugin)
    {
        try
        {
            // byte[] dllBytes = await _httpClient.GetByteArrayAsync(plugin.DllUrl);
            // plugin.LoadedAssembly = Assembly.Load(dllBytes);
            OnAssemblyLoaded.Invoke(Assembly.Load(plugin.AssemblyData));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load assembly for {plugin.Name}: {ex.Message}");
            plugin.IsEnabled = false;
        }
    }
#endregion   Lazy Loading

    
    public IEnumerable<Assembly> GetActiveAssemblies()
    {
        // var loadedAssemblies = additionalAssemblies;
        // return _plugins
        //     .Where(p => p.IsEnabled && p.AssemblyData.IsEmpty())
        //    .Select(p => Assembly.Load(p.AssemblyData!));
        var additionalAssemblies = _plugins.Where(p => p.IsEnabled && p.IsStartup).ToList();
        IEnumerable<Assembly> result = new List<Assembly>();
        foreach (var plugin in additionalAssemblies)
        {
            Assembly loadedAssembly = Assembly.Load(plugin.AssemblyData);
            if(loadedAssembly != null) result.Append(loadedAssembly);
        }
        return result;
    }
    
    public async Task LoadPluginFromApiAsync(PluginModelDto pluginDto, WebAssemblyHostBuilder builder, ILogger logger)
    {
        try
        {
            // 1. Target the Blazor WASM virtual root folder 
            // In WASM, Directory.GetCurrentDirectory() represents the app package root
            string clientRoot = Directory.GetCurrentDirectory();
            string localFilePath = Path.Combine(clientRoot, pluginDto.PluginFileName);

            // 2. Write the byte array to the client's local virtual file system
            await File.WriteAllBytesAsync(localFilePath, pluginDto.AssemblyData);

            // 3. Load the file directly into the WebAssembly runtime context
            // Using a memory stream handles dependency bridging correctly inside the browser
            bool isLoaded = AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => string.Equals(a.FullName, pluginDto.PluginFileName, StringComparison.OrdinalIgnoreCase));

            if (!isLoaded)
            {
                // using var stream = new MemoryStream(pluginDto.AssemblyData);
                Assembly loadedAssembly = Assembly.Load(pluginDto.AssemblyData);
                PluginsUtilities.InitializeModule(loadedAssembly, builder, logger);
                OnAssemblyLoaded.Invoke(loadedAssembly);
                // 2. Safeguard for Nextended / Linq expressions: 
                // Some reflection systems fail if the Assembly is missing from the current AppDomain
                // Force the app domain to recognize it by referencing a type directly
                var sampleType = loadedAssembly.GetTypes().FirstOrDefault();
                if (sampleType != null)
                {
                    Console.WriteLine($"Initialized metadata for type: {sampleType.Name}");
                }
            }
            

            // 3. Register with your App state/router list
            // Make sure the collection receiving this assembly is initialized and NOT null
            // PluginManager.AddAdditionalAssembly(loadedAssembly); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Client-side plugin load failure: {ex.Message}");
            throw;
        }
    }
}



