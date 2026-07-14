using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services.Interfaces;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services;
public class FileExplorerStateService: IFileExplorerStateService
{


    public event Action? OnChange;
    public event Action<bool>? OnToggleFileBrowserTree;
    public event Action? OnFileExplorerClearSelection;

    public event Func<ICollection<FolderModel>, Task>? OnFoldersChanged;
    public event Func<BaseExplorerItemModel, Task>? OnFolderSelectionChanged;
    public event Func<FolderModel, Task>? OnFolderCreated;
    public event Func<FolderModel, Task>? OnFolderDeleted;
    public event Func<FolderModel, Task>? OnFolderUpdated;
    public event Func<FileModel, Task>? OnFileCreated;
    public event Func<FileModel, Task>? OnFileDeleted;
    public event Func<FileModel, Task>? OnFileUpdated;

    public void NotifyStateChanged() => OnChange?.Invoke();
    public void NotifyFileBrowserTreeToogled(bool opened) => OnToggleFileBrowserTree?.Invoke(opened);
    public void NotifyFileExplorerClearSelection() => OnFileExplorerClearSelection?.Invoke();

    public async Task NotifyFoldersChanged(ICollection<FolderModel> folders) => await OnFoldersChanged?.Invoke(folders);
    public async Task NotifyFolderSelectionChanged(BaseExplorerItemModel item) => await OnFolderSelectionChanged?.Invoke(item);

    public async Task NotifyFolderCreated(FolderModel folder) => await OnFolderCreated?.Invoke(folder);
    public async Task NotifyFolderDeleted(FolderModel folder) => await OnFolderDeleted?.Invoke(folder);
    public async Task NotifyFolderUpdated(FolderModel folder) => await OnFolderUpdated?.Invoke(folder);
    public async Task NotifyFileCreated(FileModel file) => await OnFileCreated?.Invoke(file);
    public async Task NotifyFileDeleted(FileModel file) => await OnFileDeleted?.Invoke(file);
    public async Task NotifyFileUpdated(FileModel file) => await OnFileUpdated?.Invoke(file);


    public void Dispose()
    {
        OnChange = null;
        OnFolderSelectionChanged = null;
        OnFileExplorerClearSelection = null;
        OnFolderCreated = null;
        OnFolderDeleted = null;
        OnFolderUpdated = null;
        OnFileCreated = null;
        OnFileDeleted = null;
        OnFileUpdated = null;
        GC.SuppressFinalize(this);
    }

    
}
