using System.Security.Claims;
using System.Text.Encodings.Web;
using FSH.Framework.Core.Identity.Users.Abstractions;
using FSH.Starter.ElsaWorkflow.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FSH.Starter.ElsaWorkflow.Infrastructure.Auth.FSHAuthProvider;



public class CustomHeaderAuthenticationOptions : AuthenticationSchemeOptions
{
    public string HeaderName { get; set; } = "X-Custom-Auth";
    public string Realm { get; set; } = "Elsa";
}

public class CustomHeaderAuthenticationHandler : AuthenticationHandler<CustomHeaderAuthenticationOptions>
{
    private readonly IElsaUserService _userService;

    public CustomHeaderAuthenticationHandler(
        IOptionsMonitor<CustomHeaderAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IElsaUserService userService)
        : base(options, logger, encoder)
    {
        _userService = userService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey(Options.HeaderName))
        {
            return AuthenticateResult.NoResult();
        }

        var headerValue = Request.Headers[Options.HeaderName].ToString();

        // Validate the header value and get user information
        var user = await _userService.ValidateAndGetUserAsync(headerValue);

        if (user == null)
        {
            return AuthenticateResult.Fail("Invalid authentication header");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,$"{user.Id}"),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Email, $"{user.Email}")
        };

        claims.AddRange((await _userService.GetUserRolesAsync(user, CancellationToken.None)).Select(role => new Claim(ClaimTypes.Role, $"{role}")));

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers["WWW-Authenticate"] = $"{Options.HeaderName} realm=\"{Options.Realm}\"";
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    }
}
