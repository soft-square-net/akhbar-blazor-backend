
using System.Net.Mime;
using Shared.Enums;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
public interface IExplorerItemModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public bool IsReadOnly { get; set; }
    public string Path { get; set; }
    public long Size { get; set; }
    public bool IsFolder { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    // public Guid Author { get; set; }
}
