
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.FileCards;
public abstract class FileCardBase : ComponentBaseWithState, IExplorerFile
{
    [Parameter, EditorRequired] public FileModel Model { get; set; }
}
