using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorModularEngine.Core.UI.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MudBlazorLayout;

public static class MudBlazorExtensions
{
    public static void ConfigureMudBlazorLayout(this IServiceCollection services)
    {
        // 1. Discover all layout modules across referenced assemblies
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
    }
}
