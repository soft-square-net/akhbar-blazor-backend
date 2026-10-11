using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Modules.Services;

namespace FSH.Starter.Blazor.Modules.Plugins.Hooks;

internal static class BootstrapPluginsExtension
{
    internal static PluginManager Bootstrap(this PluginManager mgr, IApiClient apiClient)
    {
        
        return mgr;
    }
    
}
