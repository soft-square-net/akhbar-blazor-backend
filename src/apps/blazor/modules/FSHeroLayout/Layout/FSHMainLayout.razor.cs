using FSH.Starter.Blazor.Infrastructure.Preferences;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Shared.Enums;

namespace FSH.Starter.Blazor.Modules.FSHeroLayout.Blazor.Layout;

public partial class FSHMainLayout
{
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;
    [Parameter]
    public EventCallback<bool> OnDarkModeToggle { get; set; }
    [Parameter]
    public EventCallback<bool> OnRightToLeftToggle { get; set; }
    [Inject] public IJSRuntime JS { get; set; } = default!;

    private bool _drawerOpen;
    private bool _isDarkMode;
    private bool _isFullscreen;
    private bool _isEnglish;

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preferences)
        {
            _drawerOpen = preferences.IsDrawerOpen;
            _isDarkMode = preferences.IsDarkMode;
        }
    }

    public async Task ToggleDarkMode()
    {
        _isDarkMode = !_isDarkMode;
        await OnDarkModeToggle.InvokeAsync(_isDarkMode);
    }

    private async Task DrawerToggle()
    {
        _drawerOpen = await ClientPreferences.ToggleDrawerAsync();
    }
    private async Task LogoutAsync()
    {
        var parameters = new DialogParameters
        {
                { nameof(BlazorShared.Components.Dialogs.Logout.ContentText), "Do you want to logout from the system?"},
                { nameof(BlazorShared.Components.Dialogs.Logout.ButtonText), "Logout"},
                { nameof(BlazorShared.Components.Dialogs.Logout.Color), Color.Error}
            };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        await DialogService.ShowAsync<BlazorShared.Components.Dialogs.Logout>("Logout", parameters, options);
    }

    private void Profile()
    {
        Navigation.NavigateTo("/identity/account");
    }

    private async Task ToggleFullscreen()
    {
        await JS.InvokeVoidAsync("toggleFullScreen");
        _isFullscreen = !_isFullscreen;
        StateHasChanged();
    }

    private async Task ToggleLanguage()
    {
        CurrentCulture = CurrentCulture.StartsWith("en", StringComparison.InvariantCultureIgnoreCase) ? FSHLang.arEg.Value : FSHLang.enUS.Value;
        _isEnglish = CurrentCulture.StartsWith("en", StringComparison.InvariantCultureIgnoreCase);
        StateHasChanged();
    }
    private string CurrentCulture
    {
        get => System.Globalization.CultureInfo.CurrentCulture.Name;
        set
        {
            if (System.Globalization.CultureInfo.CurrentCulture.Name != value)
            {
                _ = ChangeCulture(value);
            }
        }
    }

    private async Task ChangeCulture(string culture)
    {
        // Save choice to local storage
        await JS.InvokeVoidAsync("localStorage.setItem", "blazorCulture", culture);

        // Force reload the app to let Program.cs apply the thread state updates
        Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
    }
}
