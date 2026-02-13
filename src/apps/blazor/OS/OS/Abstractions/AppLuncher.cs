using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.OS.Abstractions.Interfaces;
using FSH.Starter.Blazor.OS.Types;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.OS.Abstractions;
public class AppLuncher<TResult> : IAppLuncher<TResult>
{
    public string Title { get; init; }
    public string Path { get; init; }
    public string Icon { get; init; }
    public bool IsSvg { get; init; }
    public Point2D location { get; set; }
    public string TargetPath { get; init; }
    public IAppInstance<ComponentBase>? AppInstance { get; set; }

    public void CloseContextMenu()
    {
        throw new NotImplementedException();
    }

    public void LaunchApp(string targetPath = "", Func<TResult>? SetOpenerWidowsResultCallback = null)
    {
        throw new NotImplementedException();
    }

    public void OpenContextMenu()
    {
        throw new NotImplementedException();
    }
}
