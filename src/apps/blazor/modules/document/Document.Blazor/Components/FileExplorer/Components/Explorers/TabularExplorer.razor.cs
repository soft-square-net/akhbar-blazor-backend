using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers;

public partial class TabularExplorer
{
    [CascadingParameter(Name = "CurrentFolder")] public FolderModel CurrentFolder { get; set; } = default!;
}
