using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.Bucket;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.List.v1;

public sealed class ListAwsBucketHandler(
    IStorageServiceFactory storageServiceFactory,
    [FromKeyedServices("document:buckets")] IReadRepository<Bucket> repository,
    [FromKeyedServices("document:storage-accounts")] IReadRepository<StorageAccount> repositoryStorage)
    : IRequestHandler<ListBucketRequest, PagedList<BucketDTO>>
{
    public async Task<PagedList<BucketDTO>> Handle(ListBucketRequest request, CancellationToken cancellationToken)
{
    ArgumentNullException.ThrowIfNull(request);

    var spec = new ListAwsBucketSpecs(request);

        // var storageAccount = await repositoryStorage.GetByIdAsync(request.StorageAccountId, cancellationToken);
        // if (storageAccount is null)
        // {
        //     throw new KeyNotFoundException($"StorageAccount with Id {request.StorageAccountId} was not found.");
        // }
        // IBucketStorageService bucketService = storageServiceFactory.GetBucketStorageService(storageAccount.Provider);
        
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);


        return new PagedList<BucketDTO>(items, request!.PageNumber, request!.PageSize, totalCount);
}
}
