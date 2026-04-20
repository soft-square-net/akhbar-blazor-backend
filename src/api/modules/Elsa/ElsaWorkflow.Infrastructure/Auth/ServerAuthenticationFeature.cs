
using AspNetCore.Authentication.ApiKey;
using Elsa.Extensions;
using Elsa.Features.Attributes;
using Elsa.Features.Services;
using Elsa.Identity.Features;
using Elsa.Identity.Providers;
using Elsa.Requirements;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace FSH.Starter.ElsaWorkflow.Infrastructure.Auth;

[DependsOn(typeof(IdentityFeature))]
public class ServerAuthenticationFeature : DefaultAuthenticationFeature
{
    private readonly Func<AuthenticationBuilder, AuthenticationBuilder> _configureApiKeyAuthorization = (AuthenticationBuilder builder) => builder.AddApiKeyInAuthorizationHeader<DefaultApiKeyProvider>();

    public ServerAuthenticationFeature(IModule module) : base(module)
    {
    }

    public override void Apply()
    {
        base.Services.ConfigureOptions<ConfigureJwtBearerOptions>();
        base.Services.ConfigureOptions<ValidateIdentityTokenOptions>();
        AuthenticationBuilder arg = base.Services.AddAuthentication("Jwt-or-ApiKey").AddPolicyScheme("Jwt-or-ApiKey", "Jwt-or-ApiKey", delegate (PolicySchemeOptions options)
        {
            options.ForwardDefaultSelector = (HttpContext context) => (!context.Request.Headers.Authorization.Any((string x) => x.Contains("ApiKey"))) ? "Bearer" : "ApiKey";
        });
        // .AssignJwtBearer("Bearer", "elsaBearer", options => { });
        _configureApiKeyAuthorization(arg);
        base.Services.AddScoped<IAuthorizationHandler, LocalHostRequirementHandler>();
        base.Services.AddScoped<IAuthorizationHandler, LocalHostPermissionRequirementHandler>();
        base.Services.AddScoped(ApiKeyProviderType);
        base.Services.AddScoped((IServiceProvider sp) => (IApiKeyProvider)sp.GetRequiredService(ApiKeyProviderType));
        base.Services.AddAuthorization(ConfigureAuthorizationOptions);
    }

    //public override void Configure()
    //{
        
    //    base.Configure();
    //}
    
}

public static class AuthenticationBuilderExtensions
{
    public static IModule UseServerAuthentication(this IModule module, Action<ServerAuthenticationFeature>? configure = null)
    {
        module.Configure(configure);
        return module;
    }
    public static AuthenticationBuilder AssignJwtBearer(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<JwtBearerOptions> configureOptions)
    {
        if(SchemeExists(builder.Services, authenticationScheme))
            return builder;

        return builder.AddJwtBearer(authenticationScheme, displayName, configureOptions);
        //builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<JwtBearerOptions>, JwtBearerConfigureOptions>());
        //builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerPostConfigureOptions>());
        //return builder.AddScheme<JwtBearerOptions, JwtBearerHandler>(authenticationScheme, displayName, configureOptions);
    }

    public static bool SchemeExists(IServiceCollection services, string schemeName)
    {
        // Access the AuthenticationOptions to find existing schemes
        return services.Any(service =>
            service.ServiceType == typeof(IAuthenticationSchemeProvider) &&
            service.ImplementationInstance is AuthenticationOptions options &&
            options.Schemes.Any(s => s.Name == schemeName));
    }
}
