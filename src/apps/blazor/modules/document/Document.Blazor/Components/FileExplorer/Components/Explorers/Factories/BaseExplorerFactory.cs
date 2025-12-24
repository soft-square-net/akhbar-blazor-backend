
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.FileCards.Default;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.FolderCards.Default;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Factories;
public abstract  class BaseExplorerFactory : IExplorerFactory, IDisposable
{
    protected BaseExplorerFactory(FolderModel folder) { 
        CurrentFolder = folder;
    }
    public FolderModel CurrentFolder { get; }

    //public ICollection<IExplorerFolder> Subfolders { get; set; } = new List<IExplorerFolder>();
    //public ICollection<IExplorerFile> Subfiles { get; set; } = new List<IExplorerFile>();

    public static IExplorerFactory Create(FolderModel folder)
    {
        throw new NotImplementedException();
    }
    public virtual int CreateFile(ref RenderTreeBuilder b,ref int sequence,FileModel file)
    {
        b.OpenComponent(sequence++, typeof(DefaultOtherFileCard));
        b.AddComponentParameter(sequence++, "Model", file);
        b.CloseComponent();
        return sequence;
    }

    public virtual int CreateFolder(ref RenderTreeBuilder b,ref int sequence, FolderModel folder)
    {
            b.OpenComponent(sequence++, typeof(DefaultOtherFolderCard));
            b.AddComponentParameter(sequence++, "Model", folder);
            b.CloseComponent();
            return sequence;
    }

    public void Dispose()
    {
        //Subfolders.Clear();
        //Subfiles.Clear();
    }

    /// ////////////////////////////////////

    protected abstract RenderFragment<IExplorerFile> CreateAudioFile(FileModel folderModel);
    protected abstract RenderFragment<IExplorerFile> CreateCodeFile(FileModel folderModel);
    protected abstract RenderFragment<IExplorerFile> CreateDocumentFile(FileModel folderModel);
    protected abstract RenderFragment<IExplorerFile> CreateImageFile(FileModel folderModel);
    protected abstract RenderFragment<IExplorerFile> CreateOtherFile(FileModel folderModel);
    protected abstract RenderFragment<IExplorerFile> CreateVideoFile(FileModel folderModel);


    public RenderFragment Render()
    {
        RenderFragment explorer = new RenderFragment(b =>
        {
            int sequence = 0;
            b.OpenRegion(0);
            b.OpenElement(sequence++, "div");
            foreach (var item in CurrentFolder.Folders)
            {
                switch(item)
                {

                    default:
                        CreateFolder(ref b,ref sequence, item);
                        // sequence++; // For AddComponentParameter sequence number
                        break;
                };
            }
            foreach (var item in CurrentFolder.Files)
            {
                switch (item)
                {

                    default:
                        CreateFile(ref b,ref sequence, item);
                        // sequence++; // For AddComponentParameter sequence number
                        break;
                }
                ;
            }
            b.CloseElement();
            b.CloseRegion();
            // b.Dispose();
            // b = null;
        });
        return explorer;
    }

    public void RenderItem<T>(IExplorerItem<T> control, ref RenderTreeBuilder b, ref int sequence, T folder)where T : IExplorerItemModel
    {
        var concreteType = control.GetType();
        // RenderFragment frag = new RenderFragment(b =>
        // {
            b.OpenComponent(sequence++, concreteType);
            b.CloseComponent();
        // });
    }
}
