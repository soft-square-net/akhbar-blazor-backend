using System.Collections.ObjectModel;
using FSH.Starter.Blazor.Client.Layout;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.BlazorShared.Services.Interfaces;

public interface ILayoutService
{

    // ReadOnlyCollection<RenderFragment> MenuItemsList { get; }
    public LayoutComponentBase CurrentLayout { get; }
    public LayoutProperties LayoutProperties { get; }

    IReadOnlyList<LayoutComponentBase> AvailableLayouts { get; }

    public void SetLayout<T>() where T : LayoutComponentBase;
    public void SetLayout(LayoutComponentBase layout);
    public void RegisterLayout(LayoutComponentBase layout);
    public void SetLayout(LayoutProperties properties);
    public void AddMenuItem(ModuleMenu item);
    // RenderFragment ModulesMenues();
}
