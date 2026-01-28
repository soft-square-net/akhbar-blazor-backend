
using FSH.Starter.Blazor.OS.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.OS.Services.Interfaces;
public interface IAppManagerService<TDialog, TOptions, TResult>
{
    Action<IAppType<ComponentBase>> OnAppLaunched { get; set; }
    Action<IAppType<ComponentBase>> OnAppClosed { get; set; }
    /// <summary>
    /// AppManager service that maintains a list of ActiveApp instances <see cref="IAppInstance" />
    /// </summary>
    IEnumerable<IAppInstance<ComponentBase>> ActiveApps{ get; }
    /// <summary>
    /// Add to ActiveApps 
    /// Launches an application of the specified type and adds it to the collection of active applications.
    /// </summary>
    /// <param name="appType">The <see cref="Type"/> of the application to launch. Must represent a valid application type that can be
    /// instantiated.</param>
    void LaunchApp(Type appType, string targetPath = "");
    void LaunchApp(Type appType, string zctionName, IDictionary<string,object> requestParams, IOsWindow<TDialog, TOptions, TResult> openerWindow);
    /// <summary>
    /// /* Remove from ActiveApps*/
    /// Closes the specified application instance and removes it from the list of active applications.
    /// </summary>
    /// <param name="app">The application instance to close. Cannot be <c>null</c>.</param>
    void CloseApp(IAppInstance<ComponentBase> app); 
}

