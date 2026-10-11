using System.Reflection;

namespace FSH.Starter.Blazor.Modules.Services.Loader;

public interface IPluginLoaderService
{
    IReadOnlyCollection<Assembly> LoadedAssemblies { get; }
    Task<Assembly> LoadPluginAsync(string pluginKey, string url);
}
