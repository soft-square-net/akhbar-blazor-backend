using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Starter.Blazor.Modules;
using FSH.Starter.Blazor.Modules.Services;
using FSH.Starter.BlazorShared;
using FSH.Starter.BlazorShared.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Shared.FSHPlugin;

namespace FSH.Starter.Blazor.Modules.Plugins;

public static class PluginsUtilities
{
    private static List<PluginModelDto> _plugins = new();
    private static Uri _baseAddress;

    #region Load & Initialize Plugins Asseblies

    public static async Task<WebAssemblyHostBuilder> LoadModulesFromConfiguration(WebAssemblyHostBuilder builder)
    {
        var serviceProvider = builder.Services.BuildServiceProvider();
        var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("BlazorModules");

        // Get Plugin Info From Configurations
        _plugins.AddRange(PluginsUtilitiesHelper.GetPluginInfoFromConfiguration(builder));

        await LoadPluginsFromExecutingAssembly(builder, logger);

        return builder;
    }

    private static async Task LoadPluginsFromExecutingAssembly(WebAssemblyHostBuilder builder, ILogger logger)
    {
        _baseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        // var modulesAssembleyName = Assembly.GetExecutingAssembly().GetName().Name;
        foreach (var plugin in _plugins.Where(m => m.IsEnabled && m.IsStartup))
        {
            // Skip published plugins PluginManager will load it,
            // Load the Development Plugins Only
            Assembly? modulesAssembly = null;
            try
            {
                // If Not Published then it exist on server Root Load it
                if (!plugin.IsPubished) 
                {
                    // load Modules from RunTime Root Directory
                    modulesAssembly = Assembly.Load($"{plugin.PluginFileName}.dll");
                }
                else
                {
                    modulesAssembly = Assembly.Load(plugin.AssemblyData);
                }

                // cache it in  RegisteredModules to load it Later
                ModulesConstants.RegisteredModules.Add(plugin.PluginFileName, modulesAssembly);
                await InitializeModule(modulesAssembly, builder, logger);
                // Get Layout Types from Assembly LayoutComponentBase
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error loading module assembly: {plugin.Name}");
            }
        }
    }


    internal static async Task<object?> InitializeModule(Assembly ans, WebAssemblyHostBuilder builder, ILogger logger)
    {
        Type? type = ans.GetTypes().FirstOrDefault(t => typeof(IBlazorModule).IsAssignableFrom(t));

        if (type == null) return false;

        IBlazorModule? instance = (IBlazorModule?)Activator.CreateInstance(type, new object[] { logger });
        if (instance == null) return false;
        ModulesConstants.ModulesCache.Add(instance.Name, instance);
        await instance.InitializeAsync();
        await instance.ConfigureModule(builder.Services, builder);

        return instance;
    }

    #endregion


    #region Initialize Plugins Asseblies

    public static async Task<WebAssemblyHost> LazyLoadPlugins(this WebAssemblyHost host, ILogger logger)
    {
        var pluginManager = host.Services.GetRequiredService<PluginManager>();

// Fetch manifest or register initial plugins from wwwroot/plugins/
        //! pluginManager.SetBaseAddress(_baseAddress);
        // foreach (var plugin in _plugins)
        // {
        //     await pluginManager.LoadPluginAsync(
        //         id: plugin.Id,
        //         name: plugin.Name,
        //         dllRelativeUrl: plugin.DllUrl,
        //         enabledByDefault: true
        //     );
        // }


        return host;
    }

    public static async Task<WebAssemblyHost>? UsePluginsAsync(this WebAssemblyHost app)
    {
        // 1. Get Modules Loader Service
        var ModuleLoaderService = app.Services.GetService<IModulesLoader>();

        List<Type> layoutTypes = new List<Type>();

        // 2. Load Plugin Menu Component
        ModulesConstants.ModulesCache.ToList().ForEach(async kv =>
        {
            ModuleLoaderService?.AddComponent(kv.Value.ModuleMenu);
            await kv.Value.UseModuleAsync(app);
            var modulesAssembly = kv.Value.GetType().Assembly;
            // 3. Filter Assemplies and Get Layouts Plugins
            layoutTypes.AddRange(modulesAssembly.GetTypes()
                .Where(t => t.IsAssignableTo(typeof(LayoutComponentBase)) && !t.IsAbstract));
        });

        // Setup Layouts Modules
        var layoutService = app.Services.GetRequiredService<ILayoutService>();
        SetupLayouts(layoutService, layoutTypes);
        return app;
    }


    private static async Task SetupLayouts(ILayoutService layoutService, IEnumerable<Type>? layoutTypes)
    {
        foreach (var layoutType in layoutTypes)
        {
            var layoutInstance = (LayoutComponentBase)Activator.CreateInstance(layoutType);
            layoutService.RegisterLayout(layoutInstance);
            layoutService.SetLayout(layoutInstance);
            Console.WriteLine($"Layout Registered: {layoutType.FullName}");
        }
    }

    #endregion
}
