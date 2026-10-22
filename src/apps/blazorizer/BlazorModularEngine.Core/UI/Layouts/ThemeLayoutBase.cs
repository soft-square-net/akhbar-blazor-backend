using BlazorModularEngine.Core.UI.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorModularEngine.Core.UI.Layouts;

public abstract class ThemeLayoutBase :  LayoutComponentBase,  IDisposable
{
    [Inject] private ThemeManager ThemeManager { get; set; } = default!;
    [Inject] private PluginSlotRegistry SlotRegistry { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    protected override void OnInitialized()
    {
        ThemeManager.OnThemeChanged += HandleThemeChanged;
    }

    private void HandleThemeChanged()
    {
        InvokeAsync(StateHasChanged);
    }
    
    protected async Task InjectStylesheet(string stylesheetUrl )
    {
        // Execute inline JavaScript to create and append the link tag
        await JSRuntime.InvokeVoidAsync("eval", $@"
            (function() {{
                // Check if the stylesheet is already added to prevent duplicates
                if (!document.querySelector('link[href=""{stylesheetUrl}""]')) {{
                    var link = document.createElement('link');
                    link.rel = 'stylesheet';
                    link.type = 'text/css';
                    link.href = '{stylesheetUrl}';
                    document.head.appendChild(link);
                }}
            }})();
        ");
    }
    
    
    public void Dispose()
    {
        if (ThemeManager != null)
        {
            ThemeManager.OnThemeChanged -= HandleThemeChanged;
        }
    }

    public void Dispose(bool disposing)
    {
        if (disposing)
        {}
    }
}
