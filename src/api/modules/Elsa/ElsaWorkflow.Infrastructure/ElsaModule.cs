
using Carter;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.Bucket;
using FSH.Framework.Core.Storage.File;
using FSH.Framework.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Enums;

namespace FSH.Starter.WebApi.ElsaWorkflow.Infrastructure;

public static class ElsaModule
{
    public class Endpoints : ICarterModule
    {
        public Endpoints(){

        }
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var elsaGroup = app.MapGroup("elsa").WithGroupName("elsa").WithTags("elsa");
        }
    
        
    }
    public static WebApplicationBuilder RegisterElsaServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        

        return builder;
    }
    public static async Task<WebApplication> UseElsaModule(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);


        return app;
    }
}
