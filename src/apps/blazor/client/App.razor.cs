using System.Reflection;
using FSH.Starter.Blazor.Modules.Configuration;

namespace FSH.Starter.Blazor.Client;
public partial class App
{
    private readonly IModulesManager _modulesManager;
    public App(ILogger<App> logger, IModulesManager modulesManager)
    {
        _modulesManager = modulesManager;
        additionalAssemblies = _modulesManager.ModulesAssemblies.Values.ToList();
        logger.LogInformation("Registered Modules Assemblies: {Assemblies}", string.Join(", ", additionalAssemblies.Select(a => a.FullName)));
    }
    //protected readonly List<Assembly> additionalAssemblies = new List<Assembly>() {
    //  typeof(FSH.Starter.Blazor.Modules.Pages.Document.Counter).Assembly
    //};

    protected readonly List<Assembly> additionalAssemblies;
}
