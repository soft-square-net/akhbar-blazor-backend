using System.Reflection;
using FSH.Starter.Blazor.Modules.Configuration;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Services;

namespace FSH.Starter.Blazor.Client;
public partial class App
{
    private readonly ILogger<App> _logger;
    private readonly IModulesManager _modulesManager;
    public App(ILogger<App> logger, IModulesManager modulesManager)
    {
        _logger = logger;
        _modulesManager = modulesManager;
        additionalAssemblies = _modulesManager.ModulesAssemblies.Values.ToList();
        logger.LogInformation("Registered Modules Assemblies: {Assemblies}", string.Join(", ", additionalAssemblies.Select(a => a.FullName)));
    }
    //protected readonly List<Assembly> additionalAssemblies = new List<Assembly>() {
    //  typeof(FSH.Starter.Blazor.Modules.Pages.Document.Counter).Assembly
    //};

    protected readonly List<Assembly> additionalAssemblies;
    private List<Assembly> lazyLoadedAssemblies = [];
    [Inject] LazyAssemblyLoader AssemblyLoader { get; set; } = default!;

    private async Task OnNavigateAsync(NavigationContext args)
    {
        try
        {
            if (args.Path == "{PATH}")
            {
                var assemblies = await AssemblyLoader.LoadAssembliesAsync(additionalAssemblies.Select(a => a.FullName));
                lazyLoadedAssemblies.AddRange(assemblies);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {Message}", ex.Message);
        }
    }
}
