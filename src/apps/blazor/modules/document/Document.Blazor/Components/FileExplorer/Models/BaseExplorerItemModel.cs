
using System.Net.Mime;

using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using Shared.Enums;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
public class BaseExplorerItemModel : IExplorerItemModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; }
    public string Icon { get; set; } = FileExplorerIcons.DocumentFolder;
    public bool IsReadOnly { get; set; } = false;
    public string Path { get; set; }
    public long Size { get; set; }
    public bool IsFolder { get; set; } = false;
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Modified { get; set; } = DateTime.Now;
    public FolderModel Folder { get; set; }
    protected bool _selected { get; set; }
    public bool IsSelected => _selected;


    public void Select()
    {
        foreach (var folder in Folder.Folders)
        {
            folder.UnSelect();
        }
        foreach (var file in Folder.Files)
        {
            file.UnSelect();
        }
        _selected = true;
    }
    protected void UnSelect()
    {
        _selected = false;
    }
}
