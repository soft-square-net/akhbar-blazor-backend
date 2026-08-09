using System.Threading.Tasks;

namespace FSH.Starter.Blazor.Infrastructure.Auth;

public interface ICurrentUserService
{
    /// <summary>
    /// Returns the current authenticated user info or null when not authenticated.
    /// </summary>
    System.Threading.Tasks.Task<UserInfo?> GetUserInfoAsync();
}
