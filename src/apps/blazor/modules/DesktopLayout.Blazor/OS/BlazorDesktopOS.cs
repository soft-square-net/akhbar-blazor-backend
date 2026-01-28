using System;
using System.Collections.Generic;
using System.Linq;

using FSH.Starter.Blazor.OS.Abstractions;
using FSH.Starter.Blazor.OS.Services.Interfaces;
using MediatR;
using MudBlazor;

namespace FSH.Starter.Blazor.Modules.DesktopLayout.Blazor.OS;
public class BlazorDesktopOS : OsShell<MudDialog, DialogOptions, DialogResult>
{
    public BlazorDesktopOS(IMediator mediator, IAppManagerService<MudDialog, DialogOptions, DialogResult> appManager) : base(mediator, appManager)
    {
    }
}
