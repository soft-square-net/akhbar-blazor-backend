
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Dialogs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components;
public partial class FileExplorerFoldersListComponent
{
    [CascadingParameter(Name = "CurrentFolder")] public FolderModel CurrentFolder { get; set; } = default!;
    [Parameter] public List<FolderModel> Folders { get; set; } = new();
    [Parameter] public EventCallback<FolderModel> OnFolderSelected { get; set; }
    private string Search = string.Empty;

    // private void SelectFolder(FolderModel folder)
    // {
    //     OnFolderSelected.InvokeAsync(folder);
    // }

    private void ChangeFolder(FolderModel f)
    {
        CurrentFolder = f;
        OnFolderSelected.InvokeAsync(f).Wait();
    }

}
