using FSH.Starter.Blazor.OS.Types;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.OS.Abstractions.Interfaces;

public interface ISymlink<TResult> : IAppLuncher<TResult>;
public interface IAppLuncher<TResult>
{
    string Title { get; init; }
    string Path { get; init; }
    string Icon { get; init; }
    bool IsSvg { get; init; }
    Point2D location { get; set; }
    string TargetPath { get; init; } 
    IAppInstance<ComponentBase>? AppInstance { get; set; }
    
    void LaunchApp(string targetPath = "", Func<TResult>? SetOpenerWidowsResultCallback = null);
    void OpenContextMenu();
    void CloseContextMenu();
}
