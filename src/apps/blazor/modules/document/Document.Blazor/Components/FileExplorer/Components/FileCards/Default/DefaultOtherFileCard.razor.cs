using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components;
using Shared.Enums;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.FileCards.Default;

public partial class DefaultOtherFileCard : IExplorerFile
{
    [Parameter, EditorRequired] public FileModel Model { get; set; }

    

    //public string Name { get ; set ; }
    //public string Title { get ; set ; }
    //public Uri URL { get ; set ; }
    //public string Path { get ; set ; }
    //public Guid Id { get ; set ; }
    //public long Size { get ; set ; }
    //public FileType FileType { get ; set ; }
}
