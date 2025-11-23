using System.Reflection;
using FSH.Starter.Blazor.Modules.Configuration;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using static FSH.Starter.Blazor.Modules.ModulesConstants;

namespace FSH.Starter.Blazor.Modules;
public static class ModulesExtensions
{
    public static IServiceCollection ConfigureBlazorModules(this IServiceCollection services)
    {

        GetAllModulesAssemblies().ToList().ForEach(ans =>
        {
            var AssemblyName = ans.GetName().Name?.Replace(".", "", StringComparison.OrdinalIgnoreCase);
            if ( !string.IsNullOrWhiteSpace(AssemblyName) && !RegisteredModules.ContainsKey(AssemblyName))
            {
                if (ans != null)
                {
                    RegisteredModules.Add(AssemblyName, ans);
                }
            }
        });
    
        services.AddSingleton<IModulesManager>(new ModulesManager(RegisteredModules));
        return services;
    }

    public static WebAssemblyHost UseBlazorModules(this WebAssemblyHost app)
    {

        return app;
    }


    private static IEnumerable<Assembly> GetAllModulesAssemblies()
    {
        // Get all loaded assemblies in the current application domain
        // This includes the executing assembly and all referenced/linked assemblies that have been loaded
        return AppDomain.CurrentDomain.GetAssemblies()
            .Where(t => t.GetName().Name != null &&
                        t.GetName().Name!.StartsWith(ModulesGlobalNameSpace, StringComparison.OrdinalIgnoreCase)
                  );
    }
}
