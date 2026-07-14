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
    public event Action<ICollection<FolderModel>>? OnFoldersChanged;
    public event Func<BaseExplorerItemModel, Task>? OnFolderSelectionChanged;
    public event Action? OnFileExplorerClearSelection;
    public event Action<FolderModel>? OnFolderCreated;
    public event Action<FolderModel>? OnFolderDeleted;
    public event Func<FolderModel, Task>? OnFolderUpdated;
    public event Action<FileModel>? OnFileCreated;
    public event Action<FileModel>? OnFileDeleted;
    public event Action<FileModel>? OnFileUpdated;

    public void NotifyStateChanged();
    public void NotifyFileBrowserTreeToogled(bool opened);
    public void NotifyFoldersChanged(ICollection<FolderModel> folders);
    public Task NotifyFolderSelectionChanged(BaseExplorerItemModel item);
    public void NotifyFileExplorerClearSelection();
    public void NotifyFolderCreated(FolderModel folder);
    public void NotifyFolderDeleted(FolderModel folder);
    public Task NotifyFolderUpdated(FolderModel folder);
    public void NotifyFileCreated(FileModel file);
    public void NotifyFileDeleted(FileModel file);
    public void NotifyFileUpdated(FileModel file);
}
