using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FSH.Framework.Infrastructure.Auth.Jwt;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string ApiKeyHeaderName = "X-API-Key";

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
        
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
        {
            return AuthenticateResult.NoResult();
        }

        var providedApiKey = apiKeyHeaderValues.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(providedApiKey))
        {
            return AuthenticateResult.NoResult();
        }

        var apiKey = new ApiKey()
        {
            Key = "QsJbczCNysv/5SGh+U7sxedX8C07TPQPBdsnSDKZ/aE=",
            Created = DateTime.Now,
            Expires = DateTime.Now.AddDays(1),
            IsActive = true,
            Owner = "admin",
            Roles = { "admin", "SecurityRoot" }
        };

        if (apiKey == null)
        {
            return AuthenticateResult.Fail("Invalid API Key");
        }

        if (!apiKey.IsActive)
        {
            return AuthenticateResult.Fail("API Key is not active");
        }

        if (apiKey.Expires.HasValue && apiKey.Expires.Value < DateTime.UtcNow)
        {
            return AuthenticateResult.Fail("API Key has expired");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, apiKey.Owner),
            new Claim("ApiKey", providedApiKey)
        };

        claims.AddRange(apiKey.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}

public class ApiKey
{
    public string Key { get; set; }
    public string Owner { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Expires { get; set; }
    public List<string> Roles { get; set; } = new();
    public bool IsActive { get; set; } = true;
}
