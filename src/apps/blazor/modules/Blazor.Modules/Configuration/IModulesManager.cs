using System.Reflection;

namespace FSH.Starter.Blazor.Modules.Configuration;

public interface IModulesManager
{
    Dictionary<string, Assembly> ModulesAssemblies { get; }
}
