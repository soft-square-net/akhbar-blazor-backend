using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.Bucket;
using FSH.Framework.Core.Storage.Bucket.Features;
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
        
        var onlineBucket = await bucketService.GetAllBucketsAsync(new GetAllBucketsRequest(
            request.Region,
            storageAccount.AccessKey,
            storageAccount.SecretKey
        ));

        if (!onlineBucket.Buckets.Any(b => b.Name == request.BucketName))
        {
            // If Bucket does not exist online, create it
            SvcCreateBucketResponse response = await bucketService.CreateBucketAsync(new SvcCreateBucketCommand()
            {
                AccessKey = storageAccount.AccessKey,
                SecretKey = storageAccount.SecretKey,
                BucketName = request.BucketName,
                Region = request.Region,
                Tags = request.Tags ?? new Dictionary<string, string>()
            });
            var bucket = Bucket.Create(storageAccount, request.Region, request.BucketName!,response.ResourceName, request.Description);
            await repository.AddAsync(bucket, cancellationToken);
            logger.LogInformation("bucket created {BrandId}", bucket.Id);
            return new CreateBucketResponse(bucket.Id);
        }
        else 
        {
            // If Bucket exist online, check if it exist in database
            var DbBucket = await repository.SingleOrDefaultAsync(new GetBucketByNameSpec(request.BucketName), cancellationToken);
            var OnlineBucket = onlineBucket.Buckets.SingleOrDefault(b => b.Name == request.BucketName);
            if (DbBucket is null)
            {
                // If Bucket exist online but not in database, add it to database
                logger.LogInformation($"The bucket {request.BucketName} exsist but not in the database, now adding it...");
                var bucket = Bucket.Create(storageAccount, request.Region, request.BucketName!, OnlineBucket.ResourceName, request.Description);
                await repository.AddAsync(bucket, cancellationToken);
                return new CreateBucketResponse(bucket.Id);
            }
            // If Bucket exist both online and in database, throw error that Bucket already exist
            string errorMessage = $"Can't create bucket with name {request.BucketName}, Bucket already exist";
            logger.LogError(errorMessage);
            throw new InvalidOperationException(errorMessage);
        }
    }
}
