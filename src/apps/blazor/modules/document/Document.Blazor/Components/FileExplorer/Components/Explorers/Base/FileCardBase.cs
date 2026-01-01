using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Base;
public abstract class FileCardBase : ComponentBaseWithState, IExplorerFile
{
    protected bool dataLoaded { get; set; } = true;
    [Parameter, EditorRequired] public FileModel Model { get; set; }

    protected bool visible;
    
    protected bool selected;
    protected bool hover;


    public async void OpenOverlay()
    {
        visible = true;
        dataLoaded = false;
        await Task.Delay(3000);
        dataLoaded = true;
        visible = false;
        StateHasChanged();
    }

    public void ResetExample()
    {
        dataLoaded = false;
    }

#nullable enable
    private MudMenu _contextMenu = null!;

    private async Task OpenContextMenu(MouseEventArgs args)
    {
        await _contextMenu.OpenMenuAsync(args);
    }
}
