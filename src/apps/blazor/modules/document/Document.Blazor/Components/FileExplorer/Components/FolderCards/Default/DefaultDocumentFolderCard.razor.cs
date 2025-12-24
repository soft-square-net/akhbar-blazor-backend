using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Shared.Enums;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.FolderCards.Default;

public partial class DefaultDocumentFolderCard : IExplorerFolder
{
    public FolderModel Model { get; set; }
    // public FileContentType ContentType { get ; set ; }
}
