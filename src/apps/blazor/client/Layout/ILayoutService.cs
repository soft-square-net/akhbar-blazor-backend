using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.Client.Layout;

public interface ILayoutService
{

    // ReadOnlyCollection<RenderFragment> MenuItemsList { get; }

    void AddMenuItem(ModuleMenu item);
    // RenderFragment ModulesMenues();
}
