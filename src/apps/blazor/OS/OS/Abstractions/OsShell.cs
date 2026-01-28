
using System;
using FSH.Starter.Blazor.OS.Interfaces;
using FSH.Starter.Blazor.OS.Services;
using FSH.Starter.Blazor.OS.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.OS.Abstractions;
public abstract class OsShell<TDialog, TOptions, TResult> : IOsShell<TDialog, TOptions, TResult>
{

    private readonly IMediator _mediatr;
    private readonly IList<IOsWindow<TDialog, TOptions, TResult>> _windows;
    private readonly IAppManagerService<TDialog, TOptions, TResult> _appManager;
    private readonly IEnumerable<IAppType<ComponentBase>> _installedApps;

    protected OsShell(IMediator mediator, IAppManagerService<TDialog, TOptions, TResult> appManager)
    {
        _mediatr = mediator;
        _appManager = appManager;
        _windows = new List<IOsWindow<TDialog, TOptions, TResult>>();
        _installedApps = new List<IAppType<ComponentBase>>();
    }
    public IList<IOsWindow<TDialog, TOptions, TResult>> Windows => _windows;

    public IAppManagerService<TDialog, TOptions, TResult> AppManager => _appManager;

    public IMediator Mediator => _mediatr;

    public async Task<IEnumerable<IAppType<ComponentBase>>> GetAvaliableApps()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IAppType<ComponentBase>> InstalledApps => _installedApps;
}
