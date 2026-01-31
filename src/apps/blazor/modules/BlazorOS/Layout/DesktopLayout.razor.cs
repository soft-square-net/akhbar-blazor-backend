using FSH.Starter.Blazor.Infrastructure.Preferences;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Modules.BlazorOS.Layout;
public partial class DesktopLayout
{
    [Inject] private IDialogService OsDialogService { get; set; } = default!;

    [CascadingParameter(Name = "AppRouteData")]
    private RouteData? CurrentRouteData { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; } = null;
    private bool _isDarkMode = false;
    private ClientPreference? _themePreference;
    private MudTheme _currentTheme = new MudTheme();
    private bool _themeDrawerOpen;
    private bool _rightToLeft;

    protected override async Task OnInitializedAsync()
    {
        _themePreference = await ClientPreferences.GetPreference() as ClientPreference;
        if (_themePreference == null) _themePreference = new ClientPreference();
        SetCurrentTheme(_themePreference);

        Toast.Add("Like this project? ", Severity.Info, config =>
        {
            config.BackgroundBlurred = true;
            config.Icon = Icons.Custom.Brands.GitHub;
            config.Action = "Star us on Github!";
            config.ActionColor = Color.Info;
            config.OnClick = snackbar =>
            {
                Navigation.NavigateTo("https://github.com/fullstackhero/dotnet-starter-kit");
                return Task.CompletedTask;
            };
        });
    }

    private async Task ToggleDarkLightMode(bool isDarkMode)
    {
        if (_themePreference is not null)
        {
            _themePreference.IsDarkMode = isDarkMode;
            await ThemePreferenceChanged(_themePreference);
        }
    }

    private async Task ThemePreferenceChanged(ClientPreference themePreference)
    {
        SetCurrentTheme(themePreference);
        await ClientPreferences.SetPreference(themePreference);
    }

    private void SetCurrentTheme(ClientPreference themePreference)
    {
        _isDarkMode = themePreference.IsDarkMode;
        _currentTheme.PaletteLight.Primary = themePreference.PrimaryColor;
        _currentTheme.PaletteLight.Secondary = themePreference.SecondaryColor;
        _currentTheme.PaletteDark.Primary = themePreference.PrimaryColor;
        _currentTheme.PaletteDark.Secondary = themePreference.SecondaryColor;
        _currentTheme.LayoutProperties.DefaultBorderRadius = $"{themePreference.BorderRadius}px";
        _currentTheme.LayoutProperties.DefaultBorderRadius = $"{themePreference.BorderRadius}px";
        _rightToLeft = themePreference.IsRTL;
    }
}
