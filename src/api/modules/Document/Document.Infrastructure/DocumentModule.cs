using Amazon;
using Amazon.S3;
using Carter;
using Document.Infrastructure.Services;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.Bucket;
using FSH.Framework.Core.Storage.File;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Starter.WebApi.Document.Domain;
using FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
using FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Documents.v1;
using FSH.Starter.WebApi.Document.Infrastructure.Endpoints.StorageAccounts.v1;
using FSH.Starter.WebApi.Document.Infrastructure.Persistence;
using FSH.Starter.WebApi.Document.Infrastructure.Services;
using FSH.Starter.WebApi.Document.Infrastructure.Services.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Enums;
using File = FSH.Starter.WebApi.Document.Domain.File;

namespace FSH.Starter.WebApi.Document;

public static class DocumentModule
{
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("Document") {
            this.WithName("Document");
            this.WithGroupName("doucument");
            this.WithDisplayName("API Document Module");
            this.WithDescription("This moudule scope focus on hadeling files distribution throw the framework, inside local folders or even on the cloud Like AWS Buckets");
            this.WithSummary("Files distribution throw the framework");
        }
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var documentGroup = app.MapGroup("documents").WithGroupName("documents").WithTags("documents");
            documentGroup.MapDocumentCreationEndpoint();
            documentGroup.MapGetDocumentEndpoint();
            documentGroup.MapGetDocumentListEndpoint();
            documentGroup.MapDocumentUpdateEndpoint();
            documentGroup.MapDocumentDeleteEndpoint();
            // Mapping Bucket endpoints
            var bucketsGroup = app.MapGroup("buckets").WithGroupName("documents").WithTags("buckets");
            bucketsGroup.MapBucketCreationEndpoint();
            bucketsGroup.MapBucketFileCreationEndpoint();
            bucketsGroup.MapPucketFolderCreationEndpoint();
            bucketsGroup.MapBucketDeleteEndpoint();
            bucketsGroup.MapBucketDeleteFileEndpoint();
            bucketsGroup.MapBucketDeleteFolderEndpoint();
            bucketsGroup.MapBucketGetEndpoint();
            bucketsGroup.MapBucketGetFileEndpoint();
            bucketsGroup.MapBucketGetFolderEndpoint();
            bucketsGroup.MapBucketListEndpoint();
            bucketsGroup.MapBucketListFilesEndpoint();
            bucketsGroup.MapBucketListFolderEndpoint();
            bucketsGroup.MapBucketSearchEndpoint();
            bucketsGroup.MapBucketSearchFilesEndpoint();
            bucketsGroup.MapBucketSearchFolderEndpoint();
            bucketsGroup.MapBucketUpdateEndpoint();
            bucketsGroup.MapBucketUpdateFileEndpoint();
            bucketsGroup.MapBucketUpdateFolderEndpoint();
            // Mapping Storage Account endpoints
            var storageAccountGroup = app.MapGroup("storage-accounts").WithGroupName("documents").WithTags("storage-ccounts");
            storageAccountGroup.MapStorageAccountCreationEndpoint();
            storageAccountGroup.MapStorageAccountDeleteEndpoint();
            storageAccountGroup.MapGetStorageAccountEndpoint();
            storageAccountGroup.MapGetStorageAccountListEndpoint();
            storageAccountGroup.MapStorageAccountUpdateEndpoint();
        }
    
        
    }
    public static WebApplicationBuilder RegisterDocumentServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<DocumentDbContext>();
        builder.Services.AddScoped<IDbInitializer, DocumentDbInitializer>();
        
        builder.Services.AddKeyedScoped<IRepository<Domain.Document>, DocumentRepository<Domain.Document>>("document:documents");
        builder.Services.AddKeyedScoped<IReadRepository<Domain.Document>, DocumentRepository<Domain.Document>>("document:documents");
        //builder.Services.AddKeyedScoped<IRepository<File>, DocumentRepository<File>>("document:files");
        //builder.Services.AddKeyedScoped<IReadRepository<File>, DocumentRepository<File>>("document:files");
        //builder.Services.AddKeyedScoped<IRepository<Folder>, DocumentRepository<Folder>>("document:folders");
        //builder.Services.AddKeyedScoped<IReadRepository<Folder>, DocumentRepository<Folder>>("document:folders");
        builder.Services.AddKeyedScoped<IRepository<Bucket>, DocumentRepository<Bucket>>("document:buckets");
        builder.Services.AddKeyedScoped<IReadRepository<Bucket>, DocumentRepository<Bucket>>("document:buckets");
        builder.Services.AddKeyedScoped<IRepository<StorageAccount>, DocumentRepository<StorageAccount>>("document:storage-accounts");
        builder.Services.AddKeyedScoped<IReadRepository<StorageAccount>, DocumentRepository<StorageAccount>>("document:storage-accounts");
        builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
        builder.Services.AddSingleton<IExternalRefreshingAWSWithBasicCredentials, ExternalRefreshingAWSWithBasicCredentials>();
        builder.Services.AddAWSService<IAmazonS3>();

        builder.Services.AddKeyedScoped<IFileStorageService, AWSFileStorageService>(nameof(StorageProvider.AmazonS3));
        builder.Services.AddKeyedScoped<IBucketStorageService, AWSBucketStorageService>(nameof(StorageProvider.AmazonS3));
        builder.Services.AddKeyedScoped<IFileStorageService, DocumentLocalFileStorageService>(nameof(StorageProvider.Local));
        builder.Services.AddKeyedScoped<IBucketStorageService, DocumentLocalBucketStorageService>(nameof(StorageProvider.Local));
        builder.Services.AddScoped<IStorageServiceFactory, StorageServiceFactory>();
    //    builder.Services.AddScoped<IFileStorageService>(provider =>
    //    {
    //        switch (builder.Configuration.GetValue<string>("DocumentStorage:Provider"))
    //        {

    //            case "AWS":
    //                //return new AWSFileStorageService(new AmazonS3Client(
    //                //    builder.Configuration.GetValue<string>("AWS:AccessKey"),
    //                //    builder.Configuration.GetValue<string>("AWS:SecretKey"),
    //                //    RegionEndpoint.GetBySystemName(builder.Configuration.GetValue<string>("AWS:Region"))
    //                //    ));
    //                return new AWSFileStorageService(new ExternalRefreshingAWSWithBasicCredentials(), builder.Configuration);

    //            case "LOCAL":
    //                OriginOptions localOriginOptions = new OriginOptions();
    //                builder.Configuration.GetSection("LocalOriginOptions").Bind(localOriginOptions);
    //                return new DocumentLocalFileStorageService(Options.Create(localOriginOptions)); 
    //            default:
    //                OriginOptions originOptions = new OriginOptions();
    //                builder.Configuration.GetSection("originOptions").Bind(originOptions);
    //                return new DocumentLocalFileStorageService(Options.Create(originOptions));
    //        }
    //    });

        return builder;
    }
    public static WebApplication UseDocumentModule(this WebApplication app)
    {
        return app;
    }
}
