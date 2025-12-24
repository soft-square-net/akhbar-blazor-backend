using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers;

public partial class Explorer
{
    [CascadingParameter(Name ="CurrentFolder")] FolderModel CurrentFolder { get; set; }

    [EditorRequired]
    [Parameter] 
    public EnumExplorerType ExplorerType {get; set;} = EnumExplorerType.DefaultExplorer;

    private ExplorerFactory _factory;
    // private readonly Lazy<ExplorerFactory> _lazyFactory;
    protected override void OnParametersSet()
    {
        _factory = new ExplorerFactory(ExplorerType, CurrentFolder);
    }

    public async Task OnInitializeAsync()
    {

    }

    RenderFragment Render()
    {
        return _factory.GetInstance.Render();
    }
}
