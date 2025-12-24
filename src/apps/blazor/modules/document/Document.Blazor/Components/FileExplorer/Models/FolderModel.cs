
namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
public class FolderModel: BaseExplorerItemModel
{
    public FolderModel(string name)
    {
        Name = name;
        IsFolder = true;
    }
    public List<FileModel> Files { get; init; } = new();
    public List<FolderModel> Folders { get; init; } = new();
}
