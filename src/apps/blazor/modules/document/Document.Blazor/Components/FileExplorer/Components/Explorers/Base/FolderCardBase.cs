
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Base;
public abstract partial class FolderCardBase : ComponentBase, IExplorerFolder
{
    protected bool dataLoaded { get; set; } = true;
    [Parameter, EditorRequired] public FolderModel Model { get; set; }
}
