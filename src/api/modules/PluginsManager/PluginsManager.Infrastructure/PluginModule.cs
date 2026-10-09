using Carter;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Starter.WebApi.PlugindManager.Infrastructure.Persistence;
using FSH.Starter.WebApi.PluginsManager.Domain;
using FSH.Starter.WebApi.PluginsManager.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using PluginsManager.Infrastructure.Endpoints;
using Shared.Enums;
namespace FSH.Starter.WebApi.PluginManager;

public static class PluginModule
{
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("Plugin") {
            this.WithName("Plugin");
            this.WithGroupName("plugin");
            this.WithDisplayName("API Plugin Module");
            this.WithDescription("This Plugin module scope focus on handeling modules registration and subscriptions");
            this.WithSummary("Plugins Management ");
        }
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var pluginsManagerGroup = app.MapGroup("plugins").WithGroupName("plugins").WithTags("plugins");
            pluginsManagerGroup.MapSearchPluginsEndpoint();
        }
    }
    public static WebApplicationBuilder RegisterPluginServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<PluginsDbContext>();
        builder.Services.AddScoped<IDbInitializer, PluginsDbInitializer>();
        
        builder.Services.AddKeyedScoped<IRepository<Plugin>, PluginRepository<Plugin>>("plugin:plugins");
        builder.Services.AddKeyedScoped<IReadRepository<Plugin>, PluginRepository<Plugin>>("plugin:plugins");


        return builder;
    }
    public static WebApplication UsePluginModule(this WebApplication app)
    {
        return app;
    }
}
