
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using MudBlazor;
using Nextended.Core.Extensions;
using File = FSH.Starter.Blazor.Infrastructure.Api.File;
using Folder = FSH.Starter.Blazor.Infrastructure.Api.Folder;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
public class FolderModel: BaseExplorerItemModel //, IExplorerFolder
{
    private readonly IApiClient _apiClient;

    public FolderModel(Guid id, string name,Guid bucketId, FileModel[]? files= null, FolderModel[]? children = null, IApiClient apiClient = default)
    {
        Id = id;
        Name = name;
        BucketId = bucketId;
        _apiClient = apiClient;
        SetAsFolder();
        if (files is not null)
        {
            AddFiles(files);
        }
        if (children is not null)
        {
            AddFolders(children);
        }
    }
    public FolderModel(Guid id, string name, Guid bucketId, IEnumerable<File>? files = null, IEnumerable<Folder>? children = null)
    {
        Id = id;
        Name = name;
        BucketId = bucketId;
        SetAsFolder();
        if (files is not null)
        {
            AddFiles(ToFileModel(this, files).ToArray());
        }
        if (children is not null)
        {
            AddFolders(ToFolderModel(children).ToArray());
        }
    }

    #region Properties
    public Guid BucketId { get; private set; }
    public new List<FolderModel> Children => _folders;
    private List<FileModel> _files { get; init; } = new();
    public IReadOnlyList<FileModel> Files => _files.AsReadOnly();
    private List<FolderModel> _folders { get; init; } = new();
    //public IReadOnlyList<TreeItemData<FolderModel>> Folders => _folders.Select(f => new TreeItemData<FolderModel> { Value = f }).ToList().AsReadOnly();
    public List<FolderModel> Folders => _folders;

    public string AllowedExtensions { get; set; } = string.Empty;
    public bool IsExpanded { get; set; }
    #endregion  Properties

    #region Methods
    
    public async Task LoadFromDb()
    {
        var dbFolder = await _apiClient.GetBucketFolderEndpointAsync(BucketId, Id, new CancellationToken());
        if (dbFolder is not null)
        {
            _folders.Clear();
            _files.Clear();
            AddFolders(ToFolderModel(dbFolder.Folders).ToArray());
            AddFiles(ToFileModel(this, dbFolder.Files).ToArray());
        }
    }
    #endregion Methods

    #region Actions
    /* --------------------------------- Add  -------------------------------- */

    private void AddFile(FileModel file)
    {
        file.Folder = this;
        _files.Add(file);
        Size += file.Size;
    }
    public void AddFiles(FileModel[] files)
    {
        foreach (var file in files)
        {
            AddFile(file);
        }
    }
    private void AddFolder(FolderModel folder)
    {
        folder.Folder = this;
        _folders.Add(folder);
        Size += folder.Size;
    }
    public void AddFolders(FolderModel[] folders)
    {
        foreach (var folder in folders)
        {
            AddFolder(folder);
        }
    }

    /* --------------------------------- Clear  -------------------------------- */
    public async Task ClearFolders()
    {
        foreach (var folder in _folders)
        {
            RemoveFolder(folder);
        }
    }
    public async Task ClearFiles()
    {
        foreach (var file in _files)
        {
            RemoveFile(file);
        }
    }
   
    /* --------------------------------- Remove  -------------------------------- */

    public void RemoveFile(FileModel file)
    {
        file.Folder = null;
        _files.Remove(file);
    }

    public void RemoveFolder(FolderModel folder) {
        _folders.Remove(folder);
    }

    #endregion Actions

    #region Utils 
    public static FileModel ToFileModel(FolderModel parent, File file)
    {
        return new FileModel(file.Id, file.Name, (long)file.Size, file.Created, file.LastModified) { Folder = parent };
    }
    public static List<FileModel> ToFileModel(FolderModel parent, IEnumerable<File> files)
    {
        List<FileModel> result = new List<FileModel>();
        foreach (var file in files)
        {
            result.Add(ToFileModel(parent, file));
        }
        return result;
    }

    public FolderModel ToFolderModel(FolderModel parent, Folder folder)
    {
        return new FolderModel(folder.Id, folder.Name, folder.BucketId, ToFileModel(parent, folder.Files).ToArray(), ToFolderModel(folder.Children).ToArray());
    }
    public List<FolderModel> ToFolderModel(IEnumerable<Folder> folders)
    {
        List<FolderModel> result = new List<FolderModel>();
        foreach (var folder in folders)
        {
            result.Add(ToFolderModel(this, folder));
        }
        return result;
    }

    
    #endregion Utils


}
