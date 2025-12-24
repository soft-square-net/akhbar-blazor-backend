using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components;
using Shared.Enums;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.FileCards.Default;

public partial class DefaultCodeFileCard : IExplorerFile
{
    [Parameter, EditorRequired] public FileModel Model { get; set; }
}
