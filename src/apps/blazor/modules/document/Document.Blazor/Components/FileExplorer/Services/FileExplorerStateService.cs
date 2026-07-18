using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services.Interfaces;
using Nextended.Core.Extensions;
using static FSH.Starter.Blazor.Modules.Document.Blazor.FileExplorerIcons;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services;
public class FileExplorerStateService: IFileExplorerStateService
{
    private readonly ICollection<BaseExplorerItemModel> Selection = new HashSet<BaseExplorerItemModel>();
    private readonly ICollection<BaseExplorerItemModel> CommandList = new HashSet<BaseExplorerItemModel>();
    private bool _canBrowseFiles = true;
    
    public event Action? OnChange;
    public event Action<bool>? OnToggleFileBrowserTree;
    public event Func<Task>? OnFileExplorerClearSelection;

    //command 
    public event Action<string>? BeginCommand;
    public event Func<string, ICollection<BaseExplorerItemModel>, Task>? OnCommand;
    public event Action<string>? EndCommand;

    public event Func<ICollection<FolderModel>, Task>? OnFoldersChanged;
    public event Func<BaseExplorerItemModel, Task>? OnFolderSelectionChanged;
    public event Func<ICollection<BaseExplorerItemModel>, Task>? OnFileExplorerSelectionAdded;
    public event Func<ICollection<BaseExplorerItemModel>, Task>? OnFileExplorerSelectionRemoved;
    public event Func<FolderModel, Task>? OnFolderCreated;
    public event Func<FolderModel, Task>? OnFolderDeleted;
    public event Func<FolderModel, Task>? OnFolderUpdated;
    public event Func<FileModel, Task>? OnFileCreated;
    public event Func<FileModel, Task>? OnFileDeleted;
    public event Func<FileModel, Task>? OnFileUpdated;
    public event Action<ICollection<BaseExplorerItemModel>>? OnCommandListSet;
    public event Func<FolderModel, Task>? OnCurrentFolderChanged;

    public ICollection<BaseExplorerItemModel> GetSelection => Selection;

    public bool CanBrowseFiles => _canBrowseFiles;

    public void NotifySetCommandList(ICollection<BaseExplorerItemModel> items) { 
        CommandList.Clear();  
        CommandList.AddRange(items);
        OnCommandListSet.Invoke(items);
    }

    public void NotifyStateChanged() => OnChange?.Invoke();
    public void NotifyFileBrowserTreeToogled(bool opened) => OnToggleFileBrowserTree?.Invoke(opened);

    public async Task NotifyFoldersChanged(ICollection<FolderModel> folders) => await OnFoldersChanged?.Invoke(folders);
    public async Task NotifyFolderSelectionChanged(BaseExplorerItemModel item) => await OnFolderSelectionChanged?.Invoke(item);
    public async Task NotifyCurrentFolderChanged(FolderModel folder) => await OnCurrentFolderChanged.Invoke(folder);
    public async Task NotifyFolderCreated(FolderModel folder) => await OnFolderCreated?.Invoke(folder);
    public async Task NotifyFolderDeleted(FolderModel folder) => await OnFolderDeleted?.Invoke(folder);
    public async Task NotifyFolderUpdated(FolderModel folder) => await OnFolderUpdated?.Invoke(folder);
    public async Task NotifyFileCreated(FileModel file) => await OnFileCreated?.Invoke(file);
    public async Task NotifyFileDeleted(FileModel file) => await OnFileDeleted?.Invoke(file);
    public async Task NotifyFileUpdated(FileModel file) => await OnFileUpdated?.Invoke(file);
    public async Task NotifyAddToSelection(ICollection<BaseExplorerItemModel> items) { Selection.AddRange(items); await OnFileExplorerSelectionAdded.Invoke(items); }
    public async Task NotifyRemoveFromSelection(ICollection<BaseExplorerItemModel> items) { Selection.RemoveRange(items); await OnFileExplorerSelectionRemoved.Invoke(items); }
    public async Task NotifySelectionCleared() { Selection.Clear(); await OnFileExplorerClearSelection.Invoke(); }
    public void NotifyBeginCommand(string command) => BeginCommand.Invoke(command);
    public async Task NotifyOnCommand(string command) => await OnCommand.Invoke(command,CommandList);
    public void NotifyEndCommand(string command) => EndCommand.Invoke(command);



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
