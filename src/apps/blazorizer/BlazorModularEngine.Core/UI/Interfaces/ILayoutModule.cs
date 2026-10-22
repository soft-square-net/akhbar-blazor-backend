using BlazorModularEngine.Core.Common.Interfaces;

namespace BlazorModularEngine.Core.UI.Interfaces;

public interface ILayoutModule : IBlazorModule
{
    string LayoutId { get; }
    string DisplayName { get; }
    Type LayoutComponentType { get; } // The actual Razor Layout component (e.g., typeof(MudCustomLayout))
    string[] RequiredStylesheets { get; } // CSS files specific to this layout framework
    string[] RequiredScripts { get; }
}
