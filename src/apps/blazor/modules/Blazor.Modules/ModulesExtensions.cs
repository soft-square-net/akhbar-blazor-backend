using System.Reflection;
using FSH.Starter.Blazor.Modules.Configuration;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using static FSH.Starter.Blazor.Modules.ModulesConstants;
namespace FSH.Starter.Blazor.Modules;
public static class ModulesExtensions
{
    private static string ModulesNameSpace = typeof(ModulesExtensions).Namespace;
    public static IServiceCollection ConfigureBlazorModules(this IServiceCollection services)
    {
        
        GetAllLinkedProjectNamespaces().ForEach(nsa =>
        {
            var ns = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Namespace != null && t.Namespace.Equals(nsa, StringComparison.OrdinalIgnoreCase))?
                .GetTypeInfo();
            var name = ns.Assembly.GetName().FullName.Split(',')[0];
            var AssemblyName = name.Replace(".", "", StringComparison.OrdinalIgnoreCase);
            if ( !string.IsNullOrWhiteSpace(AssemblyName) && !RegisteredModules.ContainsKey(AssemblyName))
            {
                if (ns != null)
                {
                    RegisteredModules.Add(AssemblyName, ns.Assembly);
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


    private static List<string> GetAllLinkedProjectNamespaces()
    {
        // Get all loaded assemblies in the current application domain
        // This includes the executing assembly and all referenced/linked assemblies that have been loaded
        Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        // Use LINQ to get all distinct namespaces from the types in these assemblies
        var namespaces = loadedAssemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.Namespace != null && t.Namespace!.StartsWith(typeof(ModulesConstants).Namespace!)) // Filter out types without a namespace
            .Select(t => t.Namespace)
            .Distinct()
            .ToList();

        return namespaces;
    }
}
