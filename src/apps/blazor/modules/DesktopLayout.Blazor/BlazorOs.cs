using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.OS.Abstractions;
using FSH.Starter.Blazor.OS.Services.Interfaces;
using MediatR;
using MudBlazor;
using MudBlazor.Extensions.Components;
using MudBlazor.Extensions.Options;

namespace FSH.Starter.Blazor.Modules.DesktopLayout.Blazor;
internal class BlazorOs : OsShell<MudExDialog, DialogOptionsEx, DialogResult>
{
    public BlazorOs(IMediator mediator, IAppManagerService<MudExDialog, DialogOptionsEx, DialogResult> appManager) : base(mediator, appManager)
    {
    }
}
