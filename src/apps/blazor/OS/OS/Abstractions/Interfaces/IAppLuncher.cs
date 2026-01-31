using FSH.Starter.Blazor.OS.Types;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.OS.Abstractions.Interfaces;

public interface ISymlink : IAppLuncher;
public interface IAppLuncher
{
    string Title { get; }
    string Icon { get; }
    bool IsSvg { get; }
    Point2D location { get; set; }
    string TargetPath { get; init; } 
    IAppInstance<ComponentBase>? AppInstance { get; set; }
    
    void LaunchApp(string targetPath = "");
    void OpenContextMenu();
    void CloseContextMenu();
}
