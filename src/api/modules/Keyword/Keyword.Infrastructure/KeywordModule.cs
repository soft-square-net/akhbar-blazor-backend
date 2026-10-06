using Carter;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Starter.WebApi.Keyword.Application.Documents.Abstractions;
using FSH.Starter.WebApi.Keyword.Domain;
using FSH.Starter.WebApi.Keyword.Infrastructure.Persistence;
using FSH.Starter.WebApi.Keyword.Infrastructure.Search;
using FSH.Starter.WebApi.Keyword.TextAnalysis.Common.Models.SEO;
using FSH.Starter.WebApi.Keyword.TextAnalysis.Interfaces.Common;
using FSH.Starter.WebApi.Keyword.TextAnalysis.Interfaces.Services;
using FSH.Starter.WebApi.Keyword.TextAnalysis.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

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
        builder.Services.AddKeyedScoped<IRepository<Domain.Keyword>, KeywordRepository<Domain.Keyword>>(
            "keyword:keywords");
        builder.Services.AddKeyedScoped<IReadRepository<Domain.Keyword>, KeywordRepository<Domain.Keyword>>(
            "keyword:keywords");
        builder.Services.AddKeyedScoped<IRepository<Document>, KeywordRepository<Document>>("keyword:Documents");
        builder.Services.AddKeyedScoped<IReadRepository<Document>, KeywordRepository<Document>>("keyword:Documents");
        builder.Services.AddKeyedScoped<IRepository<DocumentKeyword>, KeywordRepository<DocumentKeyword>>("keyword:DocumentKeywords");
        builder.Services.AddKeyedScoped<IReadRepository<DocumentKeyword>, KeywordRepository<DocumentKeyword>>("keyword:DocumentKeywords");
        builder.Services.AddScoped<IDocumentSearchExpressionBuilder, DocumentSearchExpressionBuilder>();
        builder.Services.AddKeyedScoped<IRepository<Domain.SEODocument>, KeywordRepository<Domain.SEODocument>>("keyword:SEODocuments");
        builder.Services.AddKeyedScoped<IReadRepository<Domain.SEODocument>, KeywordRepository<Domain.SEODocument>>("keyword:SEODocuments");


        // Register other services, e.g., Lucene indexer, text analysis utilities, etc.
        // Infrastructure Layer Setup
        builder.Services.AddSingleton(new LuceneSearchService(builder.Configuration["Lucene:IndexPath"] ?? "./lucene_index"));
        builder.Services.AddTransient<IDocumentSearchIndexer, LuceneDocumentSearchIndexer>();
        builder.Services.AddTransient<ISeoAnalysisService, SeoAnalysisService>();
        builder.Services.AddHttpClient<IGrammarCheckService, LanguageToolGrammarCheckService>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["LanguageTool:Url"] ?? "https://api.languagetool.org/v2/");
        });
        return builder;
    }

    public static WebApplication UseKeywordModule(this WebApplication app)
    {
        return app;
    }
}
