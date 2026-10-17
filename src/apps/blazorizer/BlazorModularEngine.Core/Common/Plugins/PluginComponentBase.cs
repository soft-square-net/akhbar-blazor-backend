
using BlazorModularEngine.Core.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorModularEngine.Core.Common.Plugins;

public class PluginComponentBase : ComponentBase
{
    [Inject] protected IServiceProvider RootServiceProvider { get; set; } = default!;
    [Inject] protected DynamicServiceRegistry DynamicRegistry { get; set; } = default!;

    protected T GetService<T>() where T : class
    {
        // 1. Try resolving from the dynamic registry first (post-build registered plugin services)
        var dynamicService = DynamicRegistry.GetService<T>(RootServiceProvider);
        if (dynamicService != null) return dynamicService;

        // 2. Fall back to root host service provider
        return RootServiceProvider.GetService(typeof(T)) as T;
    }
}
