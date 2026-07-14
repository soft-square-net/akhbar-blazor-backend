using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services.Interfaces;
public interface IFileExplorerStateService : IDisposable
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

    public void NotifyStateChanged();
    public void NotifyFileBrowserTreeToogled(bool opened);
    public void NotifyFileExplorerClearSelection();

    public Task NotifyFoldersChanged(ICollection<FolderModel> folders);
    public Task NotifyFolderSelectionChanged(BaseExplorerItemModel item);
    public Task NotifyFolderCreated(FolderModel folder);
    public Task NotifyFolderDeleted(FolderModel folder);
    public Task NotifyFolderUpdated(FolderModel folder);
    public Task NotifyFileCreated(FileModel file);
    public Task NotifyFileDeleted(FileModel file);
    public Task NotifyFileUpdated(FileModel file);
}
