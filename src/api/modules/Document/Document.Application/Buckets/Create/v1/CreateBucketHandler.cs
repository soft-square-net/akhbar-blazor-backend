using Amazon.Runtime.Internal.Util;
using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.Bucket;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Framework.Core.Storage.File;
using FSH.Starter.WebApi.Document.Application.Documents.Search.v1;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.Create.v1;

public sealed class CreateBucketHandler( 
    ILogger<CreateBucketHandler> logger,
    IStorageServiceFactory storageServiceFactory,
    [FromKeyedServices("document:buckets")] IRepository<Bucket> repository,
    [FromKeyedServices("document:storage-accounts")] IReadRepository<StorageAccount> repositoryStorage)
    : IRequestHandler<CreateBucketCommand, CreateBucketResponse>
{
    public async Task<CreateBucketResponse> Handle(CreateBucketCommand request, CancellationToken cancellationToken)
{
        ArgumentNullException.ThrowIfNull(request);
        var storageAccount = await repositoryStorage.GetByIdAsync(request.StorageAccountId, cancellationToken);
        if (storageAccount is null)
        {
            throw new KeyNotFoundException($"StorageAccount with Id {request.StorageAccountId} was not found.");
        }
        IBucketStorageService bucketService = storageServiceFactory.GetBucketStorageService(storageAccount.Provider);
        
        var onlineBucket = await bucketService.GetAllBucketsAsync(new Framework.Core.Storage.Bucket.Features.GetAllBucketsRequest(
            request.Region,
            storageAccount.AccessKey,
            storageAccount.SecretKey
        ));
        if (!onlineBucket.BucketsNames.Contains(request.BucketName))
        {
            Framework.Core.Storage.Bucket.Features.CreateBucketResponse response = await bucketService.CreateBucketAsync(new Framework.Core.Storage.Bucket.Features.CreateBucketCommand()
            {
                AccessKey = storageAccount.AccessKey,
                SecretKey = storageAccount.SecretKey,
                BucketName = request.BucketName,
                Region = request.Region
            });
            var bucket = Bucket.Create(storageAccount, request.Region, request.key, request.BucketName!, request.Description);
            await repository.AddAsync(bucket, cancellationToken);
            logger.LogInformation("bucket created {BrandId}", bucket.Id);
            return new CreateBucketResponse(bucket.Id);
        }
        else 
        {
            var DbBucket = await repository.SingleOrDefaultAsync(new GetBucketByNameSpec(request.BucketName), cancellationToken);
            if (DbBucket is null)
            {
                logger.LogInformation($"The bucket {request.BucketName} exsist but not in the database, now adding it...");
                var bucket = Bucket.Create(storageAccount, request.Region, request.key, request.BucketName!, request.Description);
                await repository.AddAsync(bucket, cancellationToken);
                return new CreateBucketResponse(bucket.Id);
            }

            string errorMessage = $"Can't create bucket with name {request.BucketName}, Bucket already exist";
            logger.LogError(errorMessage);
            throw new InvalidOperationException(errorMessage);
        }
    }
}
