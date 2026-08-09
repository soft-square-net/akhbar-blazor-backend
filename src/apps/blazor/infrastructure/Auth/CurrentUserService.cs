using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace FSH.Starter.Blazor.Infrastructure.Auth;

public class CurrentUserService : ICurrentUserService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private UserInfo? _cached;

    public CurrentUserService(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<UserInfo?> GetUserInfoAsync()
    {
        if (_cached is not null)
            return _cached;

        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = state.User;
        if (user?.Identity?.IsAuthenticated == true)
        {
            _cached = UserInfo.FromClaimsPrincipal(user);
            return _cached;
        }

        return null;
    }
}
