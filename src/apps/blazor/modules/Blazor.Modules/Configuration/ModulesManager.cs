using System.Reflection;

namespace FSH.Starter.Blazor.Modules.Configuration;

public class ModulesManager : IModulesManager
{
    private readonly Dictionary<string, Assembly> _modulesAsseblies;

    public ModulesManager(Dictionary<string, Assembly> modulesAsseblies)
    {
        _modulesAsseblies = modulesAsseblies;
    }

    public Dictionary<string, Assembly> ModulesAssemblies => _modulesAsseblies;
}
