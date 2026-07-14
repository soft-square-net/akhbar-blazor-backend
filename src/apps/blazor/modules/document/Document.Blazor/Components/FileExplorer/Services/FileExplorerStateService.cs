using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services.Interfaces;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services;
public class FileExplorerStateService: IFileExplorerStateService
{
    public event Action? OnChange;
    public event Action<ICollection<FolderModel>>? OnFoldersChanged;
    public event Func<BaseExplorerItemModel, Task>? OnFolderSelectionChanged;
    public event Action? OnFileExplorerClearSelection;
    public event Action<FolderModel>? OnFolderCreated;
    public event Action<FolderModel>? OnFolderDeleted;
    public event Func<FolderModel, Task>? OnFolderUpdated;
    public event Action<FileModel>? OnFileCreated;
    public event Action<FileModel>? OnFileDeleted;
    public event Action<FileModel>? OnFileUpdated;
    public event Action<bool>? OnToggleFileBrowserTree;

    public void NotifyStateChanged() => OnChange?.Invoke();
    public void NotifyFoldersChanged(ICollection<FolderModel> folders) => OnFoldersChanged?.Invoke(folders);
    public async Task NotifyFolderSelectionChanged(BaseExplorerItemModel item) => await OnFolderSelectionChanged?.Invoke(item);
    public void NotifyFileExplorerClearSelection() => OnFileExplorerClearSelection?.Invoke();

    public void NotifyFolderCreated(FolderModel folder) => OnFolderCreated?.Invoke(folder);
    public void NotifyFolderDeleted(FolderModel folder) => OnFolderDeleted?.Invoke(folder);
    public async Task NotifyFolderUpdated(FolderModel folder) => OnFolderUpdated?.Invoke(folder);
    public void NotifyFileCreated(FileModel file) => OnFileCreated?.Invoke(file);
    public void NotifyFileDeleted(FileModel file) => OnFileDeleted?.Invoke(file);
    public void NotifyFileUpdated(FileModel file) => OnFileUpdated?.Invoke(file);
    public void NotifyFileBrowserTreeToogled(bool opened) => OnToggleFileBrowserTree?.Invoke(opened);
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
