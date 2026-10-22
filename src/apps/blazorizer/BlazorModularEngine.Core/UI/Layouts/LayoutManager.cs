using BlazorModularEngine.Core.UI.Interfaces;
using Microsoft.JSInterop;

namespace BlazorModularEngine.Core.UI.Layouts;

public class LayoutManager : ILayoutManager
{
    private readonly IEnumerable<ILayoutModule> _modules;
    private readonly IJSRuntime _jsRuntime;
    
    public IReadOnlyCollection<ILayoutModule> AvailableLayouts { get; }
    public ILayoutModule CurrentLayout { get; private set; }

    public event Action OnLayoutChanged;

    public LayoutManager(IEnumerable<ILayoutModule> modules, IJSRuntime jsRuntime)
    {
        _modules = modules;
        AvailableLayouts = _modules.ToList();
        
        // Default to the first available layout (e.g., MudBlazor)
        CurrentLayout = AvailableLayouts.FirstOrDefault() 
                        ?? throw new InvalidOperationException("No layouts registered.");
    }

    public async Task SetActiveLayoutAsync(string layoutId)
    {
        var target = _modules.FirstOrDefault(m => m.LayoutId == layoutId);
        if (target != null && target != CurrentLayout)
        {
            CurrentLayout = target;
            
            // Optional: Dynamically load/unload framework stylesheets if needed
            await InjectStylesheetsAsync(target.RequiredStylesheets);
            // await InjectScriptsAsync(target.RequiredScripts);
            
            
            OnLayoutChanged?.Invoke();
        }
    }

    private async Task InjectStylesheetsAsync(string[] stylesheets)
    {
        // Invoke JS to swap out stylesheet links dynamically to avoid CSS conflicts
        await _jsRuntime.InvokeVoidAsync("layoutManager.updateStylesheets", stylesheets);
    }

    private async Task InjectScriptsAsync(string[] scripts)
    {
        foreach (var script in scripts)
        {
            await _jsRuntime.InvokeVoidAsync("window.loadScript", script);
        }
    }

}
