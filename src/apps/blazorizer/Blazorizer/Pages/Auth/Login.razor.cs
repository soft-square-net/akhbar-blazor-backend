// using FSH.Starter.BlazorShared.Components;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Infrastructure.Auth;

using FSH.Starter.Blazor.Infrastructure.Notifications;
using FSH.Starter.Blazor.Infrastructure.Notifications.Users;
using FSH.Starter.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Blazorizer.Pages.Auth;

public partial class Login()
{
    //[Inject]
    //protected INotificationPublisher Notifications { get; set; } = default!;

    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    [CascadingParameter]
    public Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthenticationService authService { get; set; } = default!;

    // private FshValidation? _customValidation;

    public bool BusySubmitting { get; set; }

    private readonly TokenGenerationCommand _tokenRequest = new();
    private string TenantId { get; set; } = TenantConstants.Root.Id;
    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected async Task OnInitializedAsync()
    {
        var authState = await AuthState;
        if (authState.User.Identity?.IsAuthenticated is true)
        {
             Navigation.NavigateTo("/");
        }
    }

    private void TogglePasswordVisibility()
    {
        if (_passwordVisibility)
        {
            _passwordVisibility = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInput = InputType.Password;
        }
        else
        {
            _passwordVisibility = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInput = InputType.Text;
        }
    }

    private void FillAdministratorCredentials()
    {
        _tokenRequest.Email = TenantConstants.Root.EmailAddress;
        _tokenRequest.Password = TenantConstants.DefaultPassword;
        TenantId = TenantConstants.Root.Id;
    }

    private async Task SubmitAsync()
    {
        BusySubmitting = true;
        await authService.LoginAsync(TenantId, _tokenRequest);
        //if (await ApiHelper.ExecuteCallGuardedAsync(
        //    () => authService.LoginAsync(TenantId, _tokenRequest),
        //    // Toast,
        //    _customValidation))
        //{
        //    await Notifications.PublishAsync(new UserLoggedIn((await AuthState).User, $"Notification: Logged in as {_tokenRequest.Email}"));
        //    // Toast.Add($"Logged in as {_tokenRequest.Email}", Severity.Info);
        //}

        BusySubmitting = false;
    }
}
