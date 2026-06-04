using System.Security.Claims;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace FSH.Starter.BlazorShared;
public abstract partial class MobulePageBase: ComponentBaseWithState
{
    [CascadingParameter] protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject] protected IAuthorizationService AuthService { get; set; } = default!;

    [Inject] protected IApiClient UsersClient { get; set; } = default!;


    protected abstract string Path { get; set; }
    //protected override Task OnParametersSetAsync()
    //{
    //    return base.OnParametersSetAsync();
    //}

    //protected override async Task OnInitializedAsync()
    //{
    //    var user = (await AuthState).User;
    //    base.OnInitializedAsync();
    //}

    //protected override Task OnAfterRenderAsync(bool firstRender)
    //{
    //    return base.OnAfterRenderAsync(firstRender);
    //}
}
