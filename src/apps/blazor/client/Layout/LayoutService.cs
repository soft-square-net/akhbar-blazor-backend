using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.Client.Layout;

public class LayoutService : ILayoutService
{

    private readonly Collection<ModuleMenu> _menuItemsList = new ();
    public void AddMenuItem(ModuleMenu item)
    {
        throw new NotImplementedException();
    }

    
    public RenderFragment ModulesMenues()
    {
        throw new NotImplementedException();
    }
}

public record ModuleMenu(RenderFragment template, int order);
