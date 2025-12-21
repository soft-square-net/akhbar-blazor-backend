
namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
public record FolderModel(string Name)
{
    public List<FileModel> Files { get; init; } = new();
}
