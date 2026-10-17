
namespace BlazorModularEngine.Core.UI.Theme;


public class ThemeManager
{
    private readonly ThemeRegistry _registry;
    private ThemeTemplate _currentTemplate;

    public event Action? OnThemeChanged;

    public ThemeManager(ThemeRegistry registry)
    {
        _registry = registry;
        _currentTemplate = _registry.GetTheme("tailwind-default") ?? new ThemeTemplate();
    }

    public ThemeTemplate CurrentTemplate => _currentTemplate;
    public IEnumerable<ThemeTemplate> AvailableThemes => _registry.GetAllThemes();

    public void InstallAndApplyTheme(ThemeTemplate customTemplate)
    {
        if (customTemplate == null) throw new ArgumentNullException(nameof(customTemplate));
        _registry.RegisterTheme(customTemplate);
        _currentTemplate = customTemplate;
        OnThemeChanged?.Invoke();
    }

    public void ApplyTheme(string themeId)
    {
        var newTheme = _registry.GetTheme(themeId);
        if (newTheme != null)
        {
            _currentTemplate = newTheme;
            OnThemeChanged?.Invoke();
        }
    }
}
