
using System.Net.Mime;

using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services;
using Microsoft.AspNetCore.Components;
using Shared.Enums;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
public class BaseExplorerItemModel : IExplorerItemModel
{
    // [Inject] public IFileExplorerStateService StateService { get; set; }
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
    private bool _selected { get; set; }
    public bool IsSelected => _selected;


    public void Select()
    {
        
        _selected = true;
        // StateService.NotifyFileSelectionChanged();
        // StateService.NotifyStateChanged();
    }
    public void UnSelect()
    {
        _selected = false;
    }
}
