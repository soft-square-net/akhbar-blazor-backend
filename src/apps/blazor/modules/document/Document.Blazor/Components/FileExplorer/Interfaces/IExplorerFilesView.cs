using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;

public interface IExplorerFilesView
{
    FolderModel CurrentFolder { get; set; }
    //ICollection<IExplorerFolder> Subfolders { get; set; }
    //ICollection<IExplorerFile> Subfiles { get; set; }
    abstract static IExplorerFilesView Create(FolderModel folder);
    int CreateFolder(ref RenderTreeBuilder b,ref int sequence, FolderModel folder);
    int CreateFile(ref RenderTreeBuilder b,ref int sequence, FileModel file);
    RenderFragment Render();
    //IExplorerFile CreateAudioFile(FileModel folderModel);
    //abstract IExplorerFile CreateCodeFile(FileModel folderModel);
    //IExplorerFile CreateDocumentFile(FileModel folderModel);
    //IExplorerFile CreateImageFile(FileModel folderModel);
    //IExplorerFile CreateVideoFile(FileModel folderModel);
    //IExplorerFile CreateOtherFile(FileModel folderModel);
}
