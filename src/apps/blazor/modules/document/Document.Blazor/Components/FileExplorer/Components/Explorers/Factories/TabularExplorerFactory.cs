using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Base;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Views.Default.FileCards;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Factories;

public class TabularExplorerFactory : BaseExplorerFactory
{
    private TabularExplorerFactory(FolderModel folder) : base(folder)
    {
            
    }

    public static IExplorerFilesView Create(FolderModel folder)
    {
        return new TabularExplorerFactory(folder);
    }

    public IExplorerFolder CreateFolder(FolderModel folder)
    {
        throw new NotImplementedException();
    }
    public IExplorerFile CreateFile(FileModel folderModel)
    {
        throw new NotImplementedException();
    }



    protected override void CreateAudioFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel)
    {
        RenderItem<DefaultAudioFileCard, FileModel>(ref b, ref sequence, folderModel);
    }

    protected override void CreateCodeFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel)
    {
        RenderItem<DefaultCodeFileCard, FileModel>(ref b, ref sequence, folderModel);
    }

    protected override void CreateDocumentFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel)
    {
        RenderItem<DefaultDocumentFileCard, FileModel>(ref b, ref sequence, folderModel);
    }

    protected override void CreateImageFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel)
    {
        RenderItem<DefaultImageFileCard, FileModel>(ref b, ref sequence, folderModel);
    }

    protected override void CreateOtherFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel)
    {
        RenderItem<DefaultOtherFileCard, FileModel>(ref b, ref sequence, folderModel);
    }

    protected override void CreateVideoFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel)
    {
        RenderItem<DefaultVideoFileCard, FileModel>(ref b, ref sequence, folderModel);
    }
}
