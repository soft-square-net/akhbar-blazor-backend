using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Starter.WebApi.Document.Domain;
using FSH.Starter.WebApi.Document.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
public sealed class GetBucketHandler(
    [FromKeyedServices("document:buckets")] IReadRepository<Bucket> repository,
    ICacheService cache)
    : IRequestHandler<GetBucketRequest, BucketResponse>
{
    public async Task<BucketResponse> Handle(GetBucketRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"Bucket:{request.Id}",
            async () =>
            {
                var bucketItem = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (bucketItem == null) throw new BucketNotFoundException(request.Id);
                return new BucketResponse(bucketItem.Id,bucketItem.StorageAccount.Id,bucketItem.StorageAccount.AccountName, bucketItem.Name, bucketItem.Region, bucketItem.Description, bucketItem.Size,bucketItem.MaxSize, bucketItem.Created);
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
