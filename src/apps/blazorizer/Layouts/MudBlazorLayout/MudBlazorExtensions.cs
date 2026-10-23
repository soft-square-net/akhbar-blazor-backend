using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorModularEngine.Core.Common.Interfaces;
using BlazorModularEngine.Core.Services;
using BlazorModularEngine.Core.Services.Interfaces;
using BlazorModularEngine.Core.UI.Interfaces;
using BlazorModularEngine.Core.UI.Layouts;
using BlazorModularEngine.Core.UI.Menu;
using Microsoft.Extensions.DependencyInjection;

namespace MudBlazorLayout;

public static class MudBlazorExtensions
{
    public static void ConfigureMudBlazorLayout(this IServiceCollection services)
    {
        // ==========================================
        // 1. SCAN AND REGISTER LAYOUT MODULES
        // ==========================================
        var layoutModuleTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(ILayoutModule).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

        var discoveredModules = new List<ILayoutModule>();

        foreach (var type in layoutModuleTypes)
        {
            if (Activator.CreateInstance(type) is ILayoutModule module)
            {
                // 2. CRITICAL: Invoke the framework's service registration 
                // into the main builder.Services BEFORE building the container!
                module.ConfigureServices(services);

                // 3. Register the module instance itself so LayoutManager can consume it later
                services.AddSingleton(typeof(ILayoutModule), module);
                discoveredModules.Add(module);
            }
        }

        // ==========================================
        // 2. SCAN AND REGISTER FEATURE MODULES (IBlazorModule)
        // ==========================================
        var featureModuleTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IBlazorModule).IsAssignableFrom(p) &&
                        !p.IsInterface &&
                        !p.IsAbstract &&
                        !typeof(ILayoutModule).IsAssignableFrom(p)); // Avoid double-registering layout modules if they also inherit it

        foreach (var type in featureModuleTypes)
        {
            if (Activator.CreateInstance(type) is IBlazorModule featureModule)
            {
                // Register feature services
                featureModule.ConfigureServices(services);

                // Register instance so MenuRegistry can collect its items
                services.AddSingleton(typeof(IBlazorModule), featureModule);
            }
        }


        // 2. Theme & Layout Engine Services
        // builder.Services.AddSingleton<ThemeRegistry>();
        // builder.Services.AddSingleton<ThemeManager>();
        services.AddSingleton<ILayoutManager, LayoutManager>();
        services.AddSingleton<ILayoutModule, MudBlazorLayoutModule>();
        services.AddSingleton<IModulesLoader, ModulesLoader>();
        services.AddSingleton<IMenuRegistry, MenuRegistry>();
        // 3. Loader Engine for WASM runtime assembly streaming
        services.AddScoped<BlazorLoaderEngine>();

    }
}
