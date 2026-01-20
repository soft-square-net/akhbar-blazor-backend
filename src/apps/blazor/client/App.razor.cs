using System.Reflection;
using FSH.Starter.Blazor.Modules.Configuration;
using FSH.Starter.BlazorShared.Configurations;
using FSH.Starter.BlazorShared.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Services;

namespace FSH.Starter.Blazor.Client;
public partial class App
{
    private readonly ILogger<App> _logger;
    private readonly IModulesManager _modulesManager;
    private readonly ICollection<ModulesConfiguration>? _modules;

    public App(ILogger<App> logger, IModulesManager modulesManager, IConfiguration configuration)
    {
        _logger = logger;
        _modulesManager = modulesManager;
        _modules = configuration.GetSection("Modules").Get<ICollection<ModulesConfiguration>>();
        additionalAssemblies = _modulesManager.ModulesAssemblies.Values.ToList();
        logger.LogInformation("Registered Modules Assemblies: {Assemblies}", string.Join(", ", additionalAssemblies.Select(a => a.FullName)));
    }
    //protected readonly List<Assembly> additionalAssemblies = new List<Assembly>() {
    //  typeof(FSH.Starter.Blazor.Modules.Pages.Document.Counter).Assembly
    //};

    protected readonly List<Assembly> additionalAssemblies;
    private List<Assembly> lazyLoadedAssemblies = [];
    [Inject] LazyAssemblyLoader AssemblyLoader { get; set; } = default!;
    // [Inject] ILayoutService layoutService { get; set; } = default!;

    private async Task OnNavigateAsync(NavigationContext args)
    {
        try
        {
            // var assemblies = await AssemblyLoader.LoadAssembliesAsync(additionalAssemblies.Select(a => a.FullName));
            List<Assembly> assemblies = new();
            foreach (var module in _modules)
            {
                if ( args.Path.StartsWith(module.Path))
                {
                    assemblies.AddRange(await AssemblyLoader.LoadAssembliesAsync(new string[] { $"FSH.Starter.Blazor.Modules.{module.Name}" }));
                }
            }
            lazyLoadedAssemblies.AddRange(assemblies);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {Message}", ex.Message);
        }
    }
}
