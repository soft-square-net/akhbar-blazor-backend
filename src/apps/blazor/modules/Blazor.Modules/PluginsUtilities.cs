using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.FSHPlugin;
using FSH.Starter.Blazor.Modules;
using FSH.Starter.BlazorShared;
using FSH.Starter.BlazorShared.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.Modules;

public static class PluginsUtilities
{
    #region Load & Initialize Plugins Asseblies

    public static async Task<WebAssemblyHostBuilder> LoadModulesFromConfiguration(WebAssemblyHostBuilder builder)
    {
        var serviceProvider = builder.Services.BuildServiceProvider();
        var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("BlazorModules");
        string? jsonString = builder.Configuration.GetValue<string>("Plugins");
        List<FSHPluginResponse> modules =
           JsonSerializer.Deserialize<List<FSHPluginResponse>>(jsonString,new JsonSerializerOptions(){PropertyNameCaseInsensitive = true})
           ?? new List<FSHPluginResponse>();
        var modulesAssembleyName = Assembly.GetExecutingAssembly().GetName().Name;
        foreach (var module in modules.Where(m => m.IsEnabled))
        {
            try
            {
                var modulesAssembly = Assembly.Load($"{module.AssemblyName}.dll");
                ModulesConstants.RegisteredModules.Add(module.AssemblyName, modulesAssembly);
                object? IsInitialized = await InitializeModule(modulesAssembly, builder, logger);
                // Get Layout Types from Assembly LayoutComponentBase
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error loading module assembly: {module.Name}");
            }
        }
        
        return builder;
    }

    private static async Task<object?> InitializeModule(Assembly ans,
        WebAssemblyHostBuilder builder, ILogger logger)
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


    public static async Task<WebAssemblyHost>? UsePluginsAsync(this WebAssemblyHost app)
    {
      
        var ModuleLoaderService = app.Services.GetService<IModulesLoader>();

        List<Type> layoutTypes = new List<Type>();
        
       ModulesConstants.ModulesCache.ToList().ForEach(async kv =>
        {
            ModuleLoaderService?.AddComponent(kv.Value.ModuleMenu);
            await kv.Value.UseModuleAsync(app);
            var modulesAssembly = kv.Value.GetType().Assembly;
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
