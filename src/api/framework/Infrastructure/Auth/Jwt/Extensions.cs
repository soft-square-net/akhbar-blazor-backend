using FSH.Framework.Core.Auth.Jwt;
using FSH.Framework.Infrastructure.Auth.Policy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace FSH.Framework.Infrastructure.Auth.Jwt;
internal static class Extensions
{
    internal static IServiceCollection ConfigureJwtAuth(this IServiceCollection services)
    {
        services.AddOptions<JwtOptions>()
            .BindConfiguration(nameof(JwtOptions))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
        services.AddAuthentication(options =>
        {
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(JwtAuthConstants.FSHSchemeName, JwtBearerFSHOptions.ConfigAction)
            .AddJwtBearer(JwtAuthConstants.ElsaSchemeName, JwtBearerElsaOptions.ConfigAction);

        // Register both configuration classes


        //var authBuilder = services.AddAuthorizationBuilder().AddRequiredPermissionPolicy();
        services.TryAddEnumerable(ServiceDescriptor.Scoped<IAuthorizationHandler, RequiredPermissionAuthorizationHandler>());

        services.AddAuthorization(options =>
        {
            AuthorizationPolicy policyBuilder = 
                new AuthorizationPolicyBuilder(JwtAuthConstants.FSHSchemeName, JwtAuthConstants.ElsaSchemeName, JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireRequiredPermissions()
                .Build();

            options.DefaultPolicy = policyBuilder;
            // options.FallbackPolicy = options.GetPolicy(RequiredPermissionDefaults.PolicyName);
        });
        return services;
    }
}
