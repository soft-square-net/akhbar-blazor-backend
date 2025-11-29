using System.Collections.ObjectModel;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.Bucket;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.List.v1;

public sealed class ListAwsBucketHandler(
    IStorageServiceFactory storageServiceFactory,
    [FromKeyedServices("document:buckets")] IReadRepository<Bucket> repository,
    [FromKeyedServices("document:storage-accounts")] IReadRepository<StorageAccount> repositoryStorage)
    : IRequestHandler<ListBucketCommand, PagedList<SingleBucketResponse>>
{
    public async Task<PagedList<SingleBucketResponse>> Handle(ListBucketCommand request, CancellationToken cancellationToken)
{
    ArgumentNullException.ThrowIfNull(request);

    var spec = new ListAwsBucketSpecs(request);

        // var storageAccount = await repositoryStorage.GetByIdAsync(request.StorageAccountId, cancellationToken);
        // if (storageAccount is null)
        // {
        //     throw new KeyNotFoundException($"StorageAccount with Id {request.StorageAccountId} was not found.");
        // }
        // IBucketStorageService bucketService = storageServiceFactory.GetBucketStorageService(storageAccount.Provider);
        var storageAccount = await repositoryStorage.GetByIdAsync(request.StorageAccountId, cancellationToken);
        if (storageAccount is null)
        {
            throw new KeyNotFoundException($"StorageAccount with Id {request.StorageAccountId} was not found.");
        }
        IBucketStorageService bucketService = storageServiceFactory.GetBucketStorageService(storageAccount.Provider);

        GetAllBucketsResponse onlineBucket = await bucketService.GetAllBucketsAsync(new Framework.Core.Storage.Bucket.Features.GetAllBucketsRequest(
            request.Region,
            storageAccount.AccessKey,
            storageAccount.SecretKey
        ));
        // var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
    var items = onlineBucket.Buckets.ToList();
    var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

    return new PagedList<SingleBucketResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
}
}
