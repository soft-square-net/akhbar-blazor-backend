using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Factories;

public class DefaultExplorerFactory : BaseExplorerFactory
{
    private DefaultExplorerFactory(FolderModel folder) : base(folder)
    {
            
    }

    public static IExplorerFactory Create(FolderModel folder)
    {
        return new DefaultExplorerFactory(folder);
    }

    

    

    protected override RenderFragment<IExplorerFile> CreateAudioFile(FileModel folderModel)
    {
        throw new NotImplementedException();
    }

    protected override RenderFragment<IExplorerFile> CreateCodeFile(FileModel folderModel)
    {
        throw new NotImplementedException();
    }

    protected override RenderFragment<IExplorerFile> CreateDocumentFile(FileModel folderModel)
    {
        throw new NotImplementedException();
    }

    protected override RenderFragment<IExplorerFile> CreateImageFile(FileModel folderModel)
    {
        throw new NotImplementedException();
    }

    protected override RenderFragment<IExplorerFile> CreateOtherFile(FileModel folderModel)
    {
        throw new NotImplementedException();
    }

    protected override RenderFragment<IExplorerFile> CreateVideoFile(FileModel folderModel)
    {
        throw new NotImplementedException();
    }
}
