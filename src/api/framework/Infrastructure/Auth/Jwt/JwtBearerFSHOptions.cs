using System.Security.Claims;
using System.Text;
using FSH.Framework.Core.Auth.Jwt;
using FSH.Framework.Core.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FSH.Framework.Infrastructure.Auth.Jwt;

public class JwtBearerFSHOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    // private readonly JwtOptions _options;
    public JwtBearerFSHOptions(IOptions<JwtOptions> options)
    // public FSHJwtBearerOptions()
    {
        // _options = options.Value;

    }
    public void Configure(JwtBearerOptions options)
    {
        Configure(string.Empty, options);

    }
    public void Configure(string? name, JwtBearerOptions options)
    {
        if (name != JwtBearerDefaults.AuthenticationScheme ) //JwtAuthConstants.FSHSchemeName)
        {
            return;
        }
        ConfigAction(options);
    }

    public static readonly Action<JwtBearerOptions> ConfigAction = options =>
    {

        byte[] key = Encoding.ASCII.GetBytes("QsJbczCNysv/5SGh+U7sxedX8C07TPQPBdsnSDKZ/aE="); // get it from appsettings.json

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidIssuer = JwtAuthConstants.Issuer,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidAudience = JwtAuthConstants.Audience,
            ValidateAudience = true,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {

                context.HandleResponse();
                if (!context.Response.HasStarted)
                {
                    throw new UnauthorizedException();
                }

                return Task.CompletedTask;
            },
            OnForbidden = _ => throw new ForbiddenException(),
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                if (!string.IsNullOrEmpty(accessToken) &&
                    context.HttpContext.Request.Path.StartsWithSegments("/notifications", StringComparison.OrdinalIgnoreCase))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILogger<JwtBearerFSHOptions>>();
                logger.LogError(context.Exception, "Authentication failed");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILogger<JwtBearerFSHOptions>>();
                logger.LogInformation("Token validated successfully");
                return Task.CompletedTask;
            }
        };

    };

}
