
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using MudBlazor;
using Nextended.Core.Extensions;
using File = FSH.Starter.Blazor.Infrastructure.Api.File;
using Folder = FSH.Starter.Blazor.Infrastructure.Api.Folder;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
public class FolderModel: BaseExplorerItemModel //, IExplorerFolder
{
    public FolderModel(Guid id, string name,Guid bucketId, FileModel[]? files= null, FolderModel[]? children = null )
    {
        Id = id;
        Name = name;
        BucketId = bucketId;
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
    public Guid BucketId { get; private set; }
    public new List<FolderModel> Children => _folders;
    private List<FileModel> _files { get; init; } = new();
    public IReadOnlyList<FileModel> Files => _files.AsReadOnly();
    private List<FolderModel> _folders { get; init; } = new();
    //public IReadOnlyList<TreeItemData<FolderModel>> Folders => _folders.Select(f => new TreeItemData<FolderModel> { Value = f }).ToList().AsReadOnly();
    public List<FolderModel> Folders => _folders;

    public string AllowedExtensions { get; set; } = string.Empty;
    public bool IsExpanded { get; set; }

    public static FileModel ToFileModel(FolderModel parent,File file) {
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
    public FolderModel ToFolderModel(FolderModel parent,Folder folder) {
        return new FolderModel(folder.Id, folder.Name, folder.BucketId, ToFileModel(parent, folder.Files).ToArray(), ToFolderModel(folder.Children).ToArray());
    }
    public List<FolderModel> ToFolderModel(IEnumerable<Folder> folders)
    {
        List<FolderModel> result = new List<FolderModel>();
        foreach (var folder in folders)
        {
            result.Add(ToFolderModel(this,folder));
        }
        return result;
    }
    public void AddFolder(FolderModel folder)
    {
        folder.Folder = this;
        _folders.Add(folder);
    }
    public void AddFolders(FolderModel[] folders)
    {
        foreach (var folder in folders)
        {
            AddFolder(folder);
        }
    }
    public void RemoveFolder(FolderModel folder) {
        _folders.Remove(folder);
    }

    public void AddFile(FileModel file)
    {
        file.Folder = this;
        _files.Add(file);
    }
    public void AddFiles(FileModel[] files)
    {
        foreach (var file in files)
        {
           AddFile(file);
        }
    }
    public void RemoveFile(FileModel file) {
        file.Folder = null;
        _files.Remove(file);
    }

    public async Task LoadFolders() { /* get the folders from the data source */ }
    public async Task LoadFiles() { /* get the files from the data source */ }
    public async Task<IReadOnlyCollection<BaseExplorerItemModel>> LoadChildren() {
        if (_folders.IsNullOrEmpty()) LoadFolders(); 
        if (_files.IsNullOrEmpty()) LoadFiles(); 
        List<BaseExplorerItemModel> result = new List<BaseExplorerItemModel>();
        result.AddRange(_folders);
        result.AddRange(_files);
        return result.AsReadOnly();
    }

    // public IReadOnlyCollection<BaseExplorerItemModel> Children =>  GetChildren().Result;
    
  }
