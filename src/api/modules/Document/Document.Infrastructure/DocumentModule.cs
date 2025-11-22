using Amazon;
using Amazon.S3;
using Carter;
using Document.Infrastructure.Services;
using FSH.Framework.Core.Origin;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage.File;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Starter.WebApi.Document.Infrastructure.Endpoints.v1;
using FSH.Starter.WebApi.Document.Infrastructure.Persistence;
using FSH.Starter.WebApi.Document.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using File = FSH.Starter.WebApi.Document.Domain.File;

namespace FSH.Starter.WebApi.Document;

public static class DocumentModule
{
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("Document") { }
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var documentGroup = app.MapGroup("documents").WithTags("documents");
            documentGroup.MapDocumentCreationEndpoint();
            documentGroup.MapGetDocumentEndpoint();
            documentGroup.MapGetDocumentListEndpoint();
            documentGroup.MapDocumentUpdateEndpoint();
            documentGroup.MapDocumentDeleteEndpoint();
        }
    }
    public static WebApplicationBuilder RegisterDocumentServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<DocumentDbContext>();
        builder.Services.AddScoped<IDbInitializer, DocumentDbInitializer>();
        
        builder.Services.AddKeyedScoped<IRepository<Domain.Document>, DocumentRepository<Domain.Document>>("document:documents");
        builder.Services.AddKeyedScoped<IReadRepository<Domain.Document>, DocumentRepository<Domain.Document>>("document:documents");
        builder.Services.AddKeyedScoped<IRepository<File>, DocumentRepository<File>>("document:files");
        builder.Services.AddKeyedScoped<IReadRepository<File>, DocumentRepository<File>>("document:files");
        builder.Services.AddKeyedScoped<IRepository<Domain.Folder>, DocumentRepository<Domain.Folder>>("document:folders");
        builder.Services.AddKeyedScoped<IReadRepository<Domain.Folder>, DocumentRepository<Domain.Folder>>("document:folders");
        builder.Services.AddKeyedScoped<IRepository<Domain.Bucket>, DocumentRepository<Domain.Bucket>>("document:buckets");
        builder.Services.AddKeyedScoped<IReadRepository<Domain.Bucket>, DocumentRepository<Domain.Bucket>>("document:buckets");
        builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
        builder.Services.AddAWSService<IAmazonS3>();
        builder.Services.AddScoped<IFileStorageService>(provider =>
        {
            switch (builder.Configuration.GetValue<string>("DocumentStorage:Provider"))
            {

                case "AWS":
                    return new AWSFileStorageService(new AmazonS3Client(
                        builder.Configuration.GetValue<string>("AWS:AccessKey"),
                        builder.Configuration.GetValue<string>("AWS:SecretKey"),
                        RegionEndpoint.GetBySystemName(builder.Configuration.GetValue<string>("AWS:Region"))
                        ));
                case "LOCAL":
                    OriginOptions localOriginOptions = new OriginOptions();
                    builder.Configuration.GetSection("LocalOriginOptions").Bind(localOriginOptions);
                    return new DocumentLocalFileStorageService(Options.Create(localOriginOptions)); 
                default:
                    OriginOptions originOptions = new OriginOptions();
                    builder.Configuration.GetSection("originOptions").Bind(originOptions);
                    return new DocumentLocalFileStorageService(Options.Create(originOptions));
            }
        });

        return builder;
    }
    public static WebApplication UseDocumentModule(this WebApplication app)
    {
        return app;
    }
}
