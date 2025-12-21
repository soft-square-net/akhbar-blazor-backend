
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Dialogs;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components;

public partial class FileExplorerContentComponent
{
    [CascadingParameter(Name = "CurrentFolder")] public FolderModel CurrentFolder { get; set; } = default!;
    [Parameter] public required ICollection<FolderModel> Folders { get; set; }
    [Parameter] public EventCallback<ICollection<FolderModel>> OnFolderUpdated { get; set; }
    [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = default!;
    [Inject] ISnackbar Snackbar { get; set; } = default!;
    [Inject] IJSRuntime JSRuntime { get; set; } = default!;

    private string Breadcrumb => $"/{CurrentFolder.Name}";

    // UI refs & state
    private ElementReference fileInput;
    private bool ShowNewFolderInput;
    private string NewFolderName = string.Empty;

    async Task TriggerFileInput()
    {
        // focus click the hidden input to show native file picker
        await fileInput.FocusAsync();
        await JSRuntime.InvokeVoidAsync("eval", "document.querySelectorAll('input[type=file]')[0].click()");
    }

    void ToggleNewFolder()
    {
        ShowNewFolderInput = !ShowNewFolderInput;
        NewFolderName = string.Empty;
    }


    void CreateFolder()
    {
        var name = (NewFolderName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            Snackbar.Add("Enter a valid folder name.", Severity.Warning);
            return;
        }

        if (Folders.Any(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase)))
        {
            Snackbar.Add("A folder with that name already exists.", Severity.Warning);
            return;
        }

        var folder = new FolderModel(name);
        Folders.Add(folder);
        CurrentFolder = folder;
        ShowNewFolderInput = false;
        Snackbar.Add($"Folder '{name}' created.", Severity.Success);
    }


    async Task HandleUpload(ChangeEventArgs e)
    {
        // Using ChangeEventArgs because ElementReference click -> native input change
        // For richer usage use InputFile component with InputFileChangeEventArgs
        // We'll try to cast: in Blazor wasm you'll likely use InputFile; this minimal handler simulates
        // NOTE: If you prefer InputFile, replace markup to <InputFile OnChange="OnInputFileChange" /> and implement OnInputFileChange(InputFileChangeEventArgs)
        Snackbar.Add("Files selected (simulate upload). Implement backend upload in HandleUpload.", Severity.Info);
    }

    void Refresh()
    {
        // Replace with re-fetch from server
        Snackbar.Add("Refreshed.", Severity.Info);
        StateHasChanged();
    }

    string GetFileIcon(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return ext switch
        {
            ".jpg" or ".jpeg" or ".png" or ".gif" => Icons.Material.Filled.Image,
            ".pdf" => Icons.Material.Filled.PictureAsPdf,
            ".mp4" or ".mov" => Icons.Material.Filled.Movie,
            ".txt" or ".md" => Icons.Material.Filled.Description,
            ".doc" or ".docx" => Icons.Material.Filled.Article,
            _ => Icons.Material.Filled.InsertDriveFile
        };
    }

    async Task DownloadFile(FileModel f)
    {
        // Replace with actual file download logic (link to blob or invoking API).
        Snackbar.Add($"Simulated download: {f.Name}", Severity.Info);
    }

    async Task DeleteFile(FileModel f)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Confirm Delete",
            $"Delete file '{f.Name}'?",
            yesText: "Delete",
            noText: "Cancel",
            options: new DialogOptions { CloseButton = true }
        );

        if (result == true)
        {
            CurrentFolder.Files.Remove(f);
            Snackbar.Add($"Deleted {f.Name}", Severity.Success);
            StateHasChanged();
        }
    }

    async Task RenameFilePrompt(FileModel f)
    {
        var parameters = new DialogParameters { ["CurrentName"] = f.Name };
        var dialog = DialogService.Show<RenameDialog>("Rename file", parameters);
        var res = await dialog.Result;
        if (!res.Canceled && res.Data is string newName && !string.IsNullOrWhiteSpace(newName))
        {
            var idx = CurrentFolder.Files.FindIndex(x => x.Id == f.Id);
            if (idx >= 0)
            {
                CurrentFolder.Files[idx] = f with { Name = newName };
                Snackbar.Add($"Renamed to {newName}", Severity.Success);
            }
        }
    }

    string FormatSize(long size)
    {
        if (size >= 1 << 30) return $"{size / (1 << 30)} GB";
        if (size >= 1 << 20) return $"{size / (1 << 20)} MB";
        if (size >= 1 << 10) return $"{size / (1 << 10)} KB";
        return $"{size} B";
    }


}
