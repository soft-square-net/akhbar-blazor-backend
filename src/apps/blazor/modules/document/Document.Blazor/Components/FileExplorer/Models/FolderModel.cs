
using MudBlazor;
using Nextended.Core.Extensions;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
public class FolderModel: BaseExplorerItemModel
{
    public FolderModel(string name, FileModel[]? files= null, FolderModel[]? children = null )
    {
        Name = name;
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
    private List<FileModel> _files { get; init; } = new();
    public IReadOnlyList<FileModel> Files => _files.AsReadOnly();
    private List<FolderModel> _folders { get; init; } = new();
    public IReadOnlyList<TreeItemData<FolderModel>> Folders => _folders.Select(f => new TreeItemData<FolderModel> { Value = f }).ToList().AsReadOnly();

    public string AllowedExtensions { get; set; } = string.Empty;
    public bool IsExpanded { get; set; }
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

    public async Task LoadFolders() { }
    public async Task LoadFiles() { }
    private async Task<IReadOnlyCollection<BaseExplorerItemModel>> GetChildren() {
        if (_folders.IsNullOrEmpty()) LoadFolders(); 
        if (_files.IsNullOrEmpty()) LoadFiles(); 
        List<BaseExplorerItemModel> result = new List<BaseExplorerItemModel>();
        result.AddRange(_folders);
        result.AddRange(_files);
        return result.AsReadOnly();
    }

    public IReadOnlyCollection<BaseExplorerItemModel> Children =>  GetChildren().Result;
    
  }
