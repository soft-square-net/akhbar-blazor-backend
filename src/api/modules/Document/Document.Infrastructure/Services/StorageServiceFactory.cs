using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.Bucket;
using FSH.Framework.Core.Storage.File;
using Microsoft.Extensions.DependencyInjection;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Infrastructure.Services;
public class StorageServiceFactory: IStorageServiceFactory
{
    private readonly IServiceProvider _serviceProvider;
    // private readonly IServiceScopeFactory _serviceScopeFactory;

    // public StorageServiceFactory(IServiceScopeFactory serviceScopeFactory)
    public StorageServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        // _serviceScopeFactory = serviceScopeFactory;
    }

    public IFileStorageService GetFileStorageService(StorageProvider type)
    {
        // using IServiceScope scope = _serviceScopeFactory.CreateScope();
        // var _serviceProvider = scope.ServiceProvider;
        IFileStorageService fileStorageService = type switch
        {
            StorageProvider.AmazonS3 => _serviceProvider.GetRequiredKeyedService<IFileStorageService>(nameof(StorageProvider.AmazonS3)),
            StorageProvider.Local => _serviceProvider.GetRequiredKeyedService<IFileStorageService>(nameof(StorageProvider.Local)),
            _ => throw new ArgumentException("Invalid StorageProvider type")
        };

        

        return fileStorageService;
    }

    public IBucketStorageService GetBucketStorageService(StorageProvider type)
    {
        // using IServiceScope scope = _serviceScopeFactory.CreateScope();
        // var _serviceProvider = scope.ServiceProvider;

        IBucketStorageService bucketStorageService = type switch
        {
            StorageProvider.AmazonS3 => _serviceProvider.GetRequiredKeyedService<IBucketStorageService>(nameof(StorageProvider.AmazonS3)),
            StorageProvider.Local => _serviceProvider.GetRequiredKeyedService<IBucketStorageService>(nameof(StorageProvider.Local)),
            _ => throw new ArgumentException("Invalid StorageProvider type")
        };

        return bucketStorageService;
    }
}
