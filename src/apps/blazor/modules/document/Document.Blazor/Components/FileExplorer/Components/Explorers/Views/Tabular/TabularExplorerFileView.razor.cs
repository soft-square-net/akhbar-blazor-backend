using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Views.Tabular;
public partial class TabularExplorerFileView : IExplorerFilesView
{
    [CascadingParameter(Name = "CurrentFolder")]
    public FolderModel CurrentFolder { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; } // The default slot

    public static IExplorerFilesView Create(FolderModel folder)
    {
        throw new NotImplementedException();
    }

    public int CreateFile(ref RenderTreeBuilder b, ref int sequence, FileModel file)
    {
        throw new NotImplementedException();
    }

    public int CreateFolder(ref RenderTreeBuilder b, ref int sequence, FolderModel folder)
    {
        throw new NotImplementedException();
    }

    public RenderFragment Render()
    {
        throw new NotImplementedException();
    }
}
