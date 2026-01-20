using System.Collections.ObjectModel;
using FSH.Starter.BlazorShared.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Layout;

public class LayoutService : ILayoutService
{
    private LayoutComponentBase _currentLayout;
    private LayoutProperties _layoutProperties;
    private List<LayoutComponentBase> _availableLayouts = new();
    private readonly Collection<ModuleMenu> _menuItemsList = new ();

    public LayoutComponentBase CurrentLayout => _currentLayout;
    public LayoutProperties LayoutProperties => _layoutProperties;

    public IReadOnlyList<LayoutComponentBase> AvailableLayouts => _availableLayouts.AsReadOnly();

    public void AddMenuItem(ModuleMenu item)
    {
        throw new NotImplementedException();
    }

    
    public RenderFragment ModulesMenues()
    {
        throw new NotImplementedException();
    }

    public void RegisterLayout(LayoutComponentBase layout)
    {
        if (!_availableLayouts.Contains(layout))
        {
            _availableLayouts.Add(layout);
        }
    }

    public void SetLayout<T>() where T : LayoutComponentBase
    {
        if (_currentLayout is not T)
        {
            SetLayout(Activator.CreateInstance<T>());
        }
    }

    public void SetLayout(LayoutComponentBase layout)
    {
        _currentLayout = _availableLayouts.Where(ly => ly.GetType().Name == layout.GetType().Name).FirstOrDefault();   
        if (_currentLayout is null)
        {
            RegisterLayout(layout);
            _currentLayout = layout;
        }
    }

    public void SetLayout(LayoutProperties properties)
    {
        _layoutProperties = properties;
    }
}

public record ModuleMenu(RenderFragment template, int order);
