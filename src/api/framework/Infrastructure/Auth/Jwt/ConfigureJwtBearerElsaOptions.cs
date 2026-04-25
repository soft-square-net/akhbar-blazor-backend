using System.Security.Claims;
using System.Text;
using FSH.Framework.Core.Auth.Jwt;
using FSH.Framework.Core.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FSH.Framework.Infrastructure.Auth.Jwt;

public class ConfigureJwtBearerElsaOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    public void Configure(string? name, JwtBearerOptions options)
    {
        if (name == JwtAuthConstants.ElsaSchemeName)
        {
            options.Authority = "https://elsa-api.com";
            options.Audience = "elsa-api";
        }
    }

    public void Configure(JwtBearerOptions options) => Configure(Options.DefaultName, options);
}
