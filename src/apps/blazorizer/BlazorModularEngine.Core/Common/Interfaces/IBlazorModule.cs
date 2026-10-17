
using BlazorModularEngine.Core.Hooks;
using BlazorModularEngine.Core.UI;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorModularEngine.Core.Common.Interfaces;

/// <summary>
/// Enhanced module interface incorporating WordPress-style hooks, UI slots, and lifecycle methods.
/// </summary>

public interface IBlazorModule
{
    string ModuleName { get; }
    string Version { get; }
    void ConfigureServices(IServiceCollection services);
    void RegisterHooks(PluginHookManager hookManager);
    void RegisterUI(PluginSlotRegistry slotRegistry);
    Task InitializeAsync(IServiceProvider serviceProvider);
    Type GetRootComponentType();
}
