

namespace BlazorModularEngine.Core.UI.Theme;

public class ThemeRegistry
{
    private readonly Dictionary<string, ThemeTemplate> _themes = new(StringComparer.OrdinalIgnoreCase);

    public ThemeRegistry()
    {
        RegisterDefaultPresets();
    }

    public void RegisterTheme(ThemeTemplate theme)
    {
        if (theme == null || string.IsNullOrWhiteSpace(theme.Id)) return;
        _themes[theme.Id] = theme;
    }

    public ThemeTemplate? GetTheme(string themeId)
    {
        return _themes.TryGetValue(themeId, out var theme) ? theme : null;
    }

    public IEnumerable<ThemeTemplate> GetAllThemes() => _themes.Values;

    private void RegisterDefaultPresets()
    {
        // 1. Tailwind Default Theme
        RegisterTheme(new ThemeTemplate
        {
            Id = "tailwind-default",
            ThemeName = "Tailwind CSS (Modern Light)",
            Framework = CssFrameworkType.Tailwind,
            LayoutMode = LayoutStyleMode.StandardLeftSidebar,
            RootContainerClass = "min-h-screen flex flex-col bg-slate-50 text-slate-900 font-sans",
            HeaderClass = "bg-white border-b border-slate-200 shadow-xs z-10 sticky top-0",
            SidebarClass = "w-full md:w-64 bg-white border-r border-slate-200 p-4 space-y-4 shrink-0",
            FooterClass = "bg-white border-t border-slate-200 py-4 px-6 text-center text-sm text-slate-500 mt-auto"
        });

        // 2. Bootstrap 5 Theme
        RegisterTheme(new ThemeTemplate
        {
            Id = "bootstrap-classic",
            ThemeName = "Bootstrap 5 (Classic)",
            Framework = CssFrameworkType.Bootstrap,
            LayoutMode = LayoutStyleMode.StandardLeftSidebar,
            RootContainerClass = "d-flex flex-column min-vh-100 bg-light text-dark",
            HeaderClass = "navbar navbar-expand-lg navbar-dark bg-dark shadow-sm sticky-top",
            HeaderInnerClass = "container-fluid px-4 d-flex align-items-center justify-between w-100",
            SidebarClass = "d-flex flex-column flex-shrink-0 p-3 bg-white border-end shadow-sm" + " style=\"width: 280px;\"",
            MainContentClass = "flex-grow-1 p-4 container",
            FooterClass = "bg-white border-top py-3 text-center text-muted mt-auto"
        });

        // 3. MudBlazor Material Design Theme
        RegisterTheme(new ThemeTemplate
        {
            Id = "mudblazor-material",
            ThemeName = "MudBlazor (Material Design)",
            Framework = CssFrameworkType.MudBlazor,
            LayoutMode = LayoutStyleMode.StandardLeftSidebar,
            RootContainerClass = "mud-theme-default mud-layout",
            HeaderClass = "mud-appbar mud-appbar-fixed-top",
            SidebarClass = "mud-drawer mud-drawer-fixed mud-drawer-pos-left mud-elevation-1 pa-4",
            MainContentClass = "mud-main-content pa-6 flex-grow-1",
            FooterClass = "mud-footer pa-4 text-center border-t"
        });
    }
}
