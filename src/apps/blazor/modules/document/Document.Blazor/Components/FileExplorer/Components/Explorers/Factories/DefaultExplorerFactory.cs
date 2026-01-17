using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Base;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Views.Default.FileCards;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Views.Default.FolderCards;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components.Rendering;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Factories;

public class DefaultExplorerFactory : BaseExplorerFactory
{
    private DefaultExplorerFactory(FolderModel folder) : base(folder)
    {
            
    }

    public static IExplorerFilesView Create(FolderModel folder)
    {
        return new DefaultExplorerFactory(folder);
    }

    protected override void CreateAudioFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel)
    {
        RenderItem<DefaultAudioFileCard,FileModel>(ref b,ref sequence, folderModel);
    }

    protected override void CreateAudioFolder(ref RenderTreeBuilder b, ref int sequence, FolderModel folderModel)
    {
        RenderItem<DefaultAudioFolderCard, FolderModel>(ref b, ref sequence, folderModel);
    }

    protected override void CreateCodeFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel)
    {
        RenderItem<DefaultCodeFileCard, FileModel>(ref b, ref sequence, folderModel);
    }

    protected override void CreateCodeFolder(ref RenderTreeBuilder b, ref int sequence, FolderModel folderModel)
    {
        RenderItem<DefaultCodeFolderCard, FolderModel>(ref b, ref sequence, folderModel);
    }

    protected override void CreateDocumentFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel)
    {
        RenderItem<DefaultDocumentFileCard, FileModel>(ref b, ref sequence, folderModel);
    }

    protected override void CreateDocumentFolder(ref RenderTreeBuilder b, ref int sequence, FolderModel folderModel)
    {
        RenderItem<DefaultDocumentFolderCard, FolderModel>(ref b, ref sequence, folderModel);
    }

    protected override void CreateImageFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel)
    {
        RenderItem<DefaultImageFileCard, FileModel>(ref b, ref sequence, folderModel);
    }

    protected override void CreateImageFolder(ref RenderTreeBuilder b, ref int sequence, FolderModel folderModel)
    {
        RenderItem<DefaultImageFolderCard, FolderModel>(ref b, ref sequence, folderModel);
    }

    protected override void CreateOtherFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel)
    {
        RenderItem<DefaultOtherFileCard, FileModel>(ref b, ref sequence, folderModel);
    }

    protected override void CreateOtherFolder(ref RenderTreeBuilder b, ref int sequence, FolderModel folderModel)
    {
        RenderItem<DefaultOtherFolderCard, FolderModel>(ref b, ref sequence, folderModel);
    }

    protected override void CreateVideoFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel)
    {
        RenderItem<DefaultVideoFileCard, FileModel>(ref b, ref sequence, folderModel);
    }

    protected override void CreateVideoFolder(ref RenderTreeBuilder b, ref int sequence, FolderModel folderModel)
    {
        RenderItem<DefaultVideoFolderCard, FolderModel>(ref b, ref sequence, folderModel);
    }
}
