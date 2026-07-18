using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services.Interfaces;
public interface IFileExplorerStateService : IDisposable
{
    public bool CanBrowseFiles { get; }

    public event Action? OnChange;
    public event Action<bool>? OnToggleFileBrowserTree;
    public event Func<Task>? OnFileExplorerClearSelection;

    public event Func<ICollection<FolderModel>, Task>? OnFoldersChanged;
    public event Func<BaseExplorerItemModel, Task>? OnFolderSelectionChanged;
    public event Func<FolderModel, Task>? OnCurrentFolderChanged;
    public event Func<FolderModel, Task>? OnFolderCreated;
    public event Func<FolderModel, Task>? OnFolderDeleted;
    public event Func<FolderModel, Task>? OnFolderUpdated;
    public event Func<FileModel, Task>? OnFileCreated;
    public event Func<FileModel, Task>? OnFileDeleted;
    public event Func<FileModel, Task>? OnFileUpdated;


    //command 
    public event Action<string>? BeginCommand;
    public event Func<string, ICollection<BaseExplorerItemModel>, Task>? OnCommand;
    public event Action<string>? EndCommand;

    public event Func<ICollection<BaseExplorerItemModel>, Task>? OnFileExplorerSelectionAdded;
    public event Func<ICollection<BaseExplorerItemModel>, Task>? OnFileExplorerSelectionRemoved;
    public event Action<ICollection<BaseExplorerItemModel>>? OnCommandListSet;
    public ICollection<BaseExplorerItemModel> GetSelection { get; }
    public void NotifySetCommandList(ICollection<BaseExplorerItemModel> items);

    public void NotifyStateChanged();
    public void NotifyFileBrowserTreeToogled(bool opened);
    public Task NotifyAddToSelection(ICollection<BaseExplorerItemModel> items);
    public Task NotifyRemoveFromSelection(ICollection<BaseExplorerItemModel> items);
    public Task NotifySelectionCleared();

    public Task NotifyFoldersChanged(ICollection<FolderModel> folders);

    public Task NotifyFolderSelectionChanged(BaseExplorerItemModel item);
    public Task NotifyCurrentFolderChanged(FolderModel folder);

    public Task NotifyFolderCreated(FolderModel folder);
    public Task NotifyFolderDeleted(FolderModel folder);
    public Task NotifyFolderUpdated(FolderModel folder);
    public Task NotifyFileCreated(FileModel file);
    public Task NotifyFileDeleted(FileModel file);
    public Task NotifyFileUpdated(FileModel file);

    public void NotifyBeginCommand(string command);
    public Task NotifyOnCommand(string command);
    public void NotifyEndCommand(string command);


}
