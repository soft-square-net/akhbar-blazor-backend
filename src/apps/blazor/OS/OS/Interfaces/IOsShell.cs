
/**
 * Represents the shell interface for the operating system.
 * Represents the Desktop environment.
 */

using FSH.Starter.Blazor.OS.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.OS.Interfaces;
public interface IOsShell<TDialog, TOptions, TResult>
{
    /// <summary>
    /// Registers a notification callback that gets invoked when an application sends a notification.
    /// The symlink generate a new App Window an send it withe the reqired Action and ites params 
    /// and set the openerWindow to the current window.
    /// the 
    /// notify function parameters:
    /// ex:var  resultParams = Notefy(IOsWindow targetWindow, string actionName, IDictionary<string, object> actionParams);
    /// </summary>
    // IList<Func<IOsWindow,string, IDictionary<string, object>, IDictionary<string,object>>> NotifyApp { get; }
    IList<IOsWindow<TDialog, TOptions, TResult>> Windows { get; }
    IAppManagerService<TDialog, TOptions, TResult> AppManager { get; }

    IMediator Mediator { get; }
    IEnumerable<IAppType<ComponentBase>> InstalledApps { get; }
    Task<IEnumerable<IAppType<ComponentBase>>> GetAvaliableApps();

}

