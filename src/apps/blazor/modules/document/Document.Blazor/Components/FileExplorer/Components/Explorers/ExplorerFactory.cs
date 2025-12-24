using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Factories;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers;

public class ExplorerFactory : IDisposable
{
    private readonly EnumExplorerType _explorerType;
    private readonly FolderModel? _folder;
    private readonly IExplorerFactory _explorerFactory;

    public ExplorerFactory(EnumExplorerType explorerType, FolderModel? folder)
    {
        _explorerType = explorerType;
        _folder = folder;
        _explorerFactory = CreateInstance;
    }

    public IExplorerFactory GetInstance => _explorerFactory;
    private IExplorerFactory CreateInstance => _explorerType switch
     {
          EnumExplorerType.DefaultExplorer => DefaultExplorerFactory.Create(_folder),
          EnumExplorerType.TabularExplorer => TabularExplorerFactory.Create(_folder),
          _ => DefaultExplorerFactory.Create(_folder)
     };

    public RenderFragment Render()
    {
        return _explorerFactory.Render();
    }
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
