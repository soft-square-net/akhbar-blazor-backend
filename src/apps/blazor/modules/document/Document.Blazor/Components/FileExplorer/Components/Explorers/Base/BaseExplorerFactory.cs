
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Views.Default.FileCards;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Views.Default.FolderCards;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Shared.Enums;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Base;
public abstract class BaseExplorerFactory : IExplorerFilesView, IDisposable
{
    private bool _disposedValue;
    
    protected BaseExplorerFactory(FolderModel folder)
    {
        CurrentFolder = folder;
    }
    public FolderModel CurrentFolder { get; set; }

    //public ICollection<IExplorerFolder> Subfolders { get; set; } = new List<IExplorerFolder>();
    //public ICollection<IExplorerFile> Subfiles { get; set; } = new List<IExplorerFile>();

    public static IExplorerFilesView Create(FolderModel folder)
    {
        throw new NotImplementedException();
    }
    public virtual int CreateFile(ref RenderTreeBuilder b, ref int sequence, FileModel file)
    {
        b.OpenComponent(sequence++, typeof(DefaultOtherFileCard));
        b.AddComponentParameter(sequence++, "Model", file);
        b.CloseComponent();
        return sequence;
    }

    public virtual int CreateFolder(ref RenderTreeBuilder b, ref int sequence, FolderModel folder)
    {
        b.OpenComponent(sequence++, typeof(DefaultOtherFolderCard));
        b.AddComponentParameter(sequence++, "Model", folder);
        b.CloseComponent();
        return sequence;
    }

 

    /// ////////////////////////////////////

    protected abstract void CreateAudioFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel);
    protected abstract void CreateCodeFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel);
    protected abstract void CreateDocumentFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel);
    protected abstract void CreateImageFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel);
    protected abstract void CreateOtherFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel);
    protected abstract void CreateVideoFile(ref RenderTreeBuilder b, ref int sequence, FileModel folderModel);


    protected abstract void CreateAudioFolder(ref RenderTreeBuilder b, ref int sequence, FolderModel folderModel);
    protected abstract void CreateCodeFolder(ref RenderTreeBuilder b, ref int sequence, FolderModel folderModel);
    protected abstract void CreateDocumentFolder(ref RenderTreeBuilder b, ref int sequence, FolderModel folderModel);
    protected abstract void CreateImageFolder(ref RenderTreeBuilder b, ref int sequence, FolderModel folderModel);
    protected abstract void CreateOtherFolder(ref RenderTreeBuilder b, ref int sequence, FolderModel folderModel);
    protected abstract void CreateVideoFolder(ref RenderTreeBuilder b, ref int sequence, FolderModel folderModel);

    public virtual RenderFragment Render()
    {
        RenderFragment explorer = new RenderFragment(b =>
        {
            int sequence = 0;
            b.OpenRegion(0);
            // b.OpenElement(sequence++, "div");
            foreach (var item in CurrentFolder.Folders)
            {
                switch (item.GetFileType())
                {
                    case FileType.Audio:
                        CreateAudioFolder(ref b, ref sequence, item);
                        break;
                    case FileType.Code:
                        CreateCodeFolder(ref b, ref sequence, item);
                        break;
                    case FileType.Document:
                        CreateDocumentFolder(ref b, ref sequence, item);
                        break;
                    case FileType.Image:
                        CreateImageFolder(ref b, ref sequence, item);
                        break;
                    case FileType.Video:
                        CreateVideoFolder(ref b, ref sequence, item);
                        break;
                    case FileType.Other:
                        CreateOtherFolder(ref b, ref sequence, item);
                        break;
                    default:
                        CreateFolder(ref b, ref sequence, item);
                        // sequence++; // For AddComponentParameter sequence number
                        break;
                }
                ;
            }
            foreach (var item in CurrentFolder.Files)
            {
                switch (item.GetFileType())
                {
                    case FileType.Audio:
                        CreateAudioFile(ref b, ref sequence, item);
                        break;
                    case FileType.Code:
                        CreateCodeFile(ref b, ref sequence, item);
                        break;
                    case FileType.Document:
                        CreateDocumentFile(ref b, ref sequence, item);
                        break;
                    case FileType.Image:
                        CreateImageFile(ref b, ref sequence, item);
                        break;
                    case FileType.Video:
                        CreateVideoFile(ref b, ref sequence, item);
                        break;
                    case FileType.Other:
                        CreateOtherFile(ref b, ref sequence, item);
                        break;
                    default:
                        CreateFile(ref b, ref sequence, item);
                        break;
                }
                ;
            }
            // b.CloseElement();
            b.CloseRegion();
            // b.Dispose();
            // b = null;
        });
        return explorer;
    }

    protected void RenderItem<T,TModel>(ref RenderTreeBuilder b, ref int sequence, TModel model) where T:IExplorerItem<TModel> , new() where TModel : BaseExplorerItemModel
    {
        // var concreteType = control.GetType();
        // RenderFragment frag = new RenderFragment(b =>
        // {
        b.OpenComponent(sequence++, typeof(T));
        b.AddComponentParameter(sequence++, "Model", model);
        b.CloseComponent();
        // });
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~BaseExplorerFactory()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
