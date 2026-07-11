
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Dialogs;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Mapster;
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
    public string SelectedFolder = string.Empty;
    public bool ReadOnly = false;
    public SelectionMode SelectionMode = SelectionMode.SingleSelection;
    // private void SelectFolder(FolderModel folder)
    // {
    //     OnFolderSelected.InvokeAsync(folder);
    // }

    private void ChangeFolder(FolderModel f)
    {
        CurrentFolder = f;
        OnFolderSelected.InvokeAsync(f).Wait();
    }



    public DirectoryInfo? FindDirectoryRecursive(string currentPath, string targetFolderName)
    {
        try
        {
            var currentDir = new DirectoryInfo(currentPath);

            // 1. Check if the current folder is the one we are looking for
            if (string.Equals(currentDir.Name, targetFolderName, StringComparison.OrdinalIgnoreCase))
            {
                return currentDir;
            }

            // 2. Loop through subdirectories and search inside them recursively
            foreach (var subDir in currentDir.GetDirectories())
            {
                var result = FindDirectoryRecursive(subDir.FullName, targetFolderName);

                // If the target was found deeper in the tree, pass it back up
                if (result != null)
                {
                    return result;
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Skip folders that the application doesn't have permission to read
        }
        catch (DirectoryNotFoundException)
        {
            // Handle case where a directory might be deleted during the scan
        }

        // Return null if the folder wasn't found in this branch
        return null;
    }
    private void HandleNodeSelected(TreeItemData<FolderModel> node)
    {
        ChangeFolder(node.Value!);
    }
}
