
using Carter;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Keyword.Domain;
using FSH.Starter.WebApi.Keyword.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Starter.WebApi.Keyword.Infrastructure.Endpoints.Documents.v1;
using Microsoft.AspNetCore.Http;
namespace FSH.Starter.WebApi.Keyword.Infrastructure;

public static class KeywordModule
{
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("keyword") { }
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var productGroup = app.MapGroup("keyword").WithGroupName("keyword").WithTags("keywords");
            //productGroup.MapProductCreationEndpoint();
            //productGroup.MapGetProductEndpoint();
            //productGroup.MapGetProductListEndpoint();
            //productGroup.MapProductUpdateEndpoint();
            //productGroup.MapProductDeleteEndpoint();

            var brandGroup = app.MapGroup("Keyphrase").WithGroupName("keyword").WithTags("Keyphrases");
            //brandGroup.MapBrandCreationEndpoint();
            //brandGroup.MapGetBrandEndpoint();
            //brandGroup.MapGetBrandListEndpoint();
            //brandGroup.MapBrandUpdateEndpoint();
            //brandGroup.MapBrandDeleteEndpoint();
        }
    }
    public static WebApplicationBuilder RegisterKeywordServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<KeywordDbContext>();
        builder.Services.AddScoped<IDbInitializer, KeywordDbInitializer>();
        builder.Services.AddKeyedScoped<IRepository<Domain.Keyword>, KeywordRepository<Domain.Keyword>>("keyword:keywords");
        builder.Services.AddKeyedScoped<IReadRepository<Domain.Keyword>, KeywordRepository<Domain.Keyword>>("keyword:keywords");
        builder.Services.AddKeyedScoped<IRepository<Document>, KeywordRepository<Document>>("keyword:Documents");
        builder.Services.AddKeyedScoped<IReadRepository<Document>, KeywordRepository<Document>>("keyword:Documents");
        return builder;
    }
    public static WebApplication UseKeywordModule(this WebApplication app)
    {
        return app;
    }
}
