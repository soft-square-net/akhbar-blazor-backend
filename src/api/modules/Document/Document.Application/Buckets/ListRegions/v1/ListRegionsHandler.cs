using System.Collections.Generic;
using System.Collections.ObjectModel;

// using Amazon;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.Bucket;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Document.Application.Buckets.ListRegions.v1;
public sealed class ListRegionsHandler(
    [FromKeyedServices("document:storage-accounts")] IReadRepository<StorageAccount> repository,
    IStorageServiceFactory storageServiceFactory)
    : IRequestHandler<ListRegionsRequest, PagedList<RegionResponse>>
{
    public async Task<PagedList<RegionResponse>> Handle(ListRegionsRequest request, CancellationToken cancellationToken)
    {
        var storageAccount = await repository.GetByIdAsync(request.StorageAccountId, cancellationToken).ConfigureAwait(false);
        if (storageAccount is null)
        {
            throw new KeyNotFoundException($"StorageAccount with Id {request.StorageAccountId} was not found.");
        }

        IBucketStorageService bucketService = storageServiceFactory.GetBucketStorageService(storageAccount.Provider);

        var regions = await bucketService.ListRegionsAsync(new SvcListRegionsCommand());
        if (!string.IsNullOrWhiteSpace(request.Keyword)) {
            regions = regions.Where(r => r.DisplayName.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        ReadOnlyCollection<RegionResponse> items = regions.Select(r => new RegionResponse { SystemName = r.SystemName, DisplayName = r.DisplayName }).ToList().AsReadOnly();
        return new PagedList<RegionResponse>( items, request!.PageNumber, request!.PageSize, items.Count);
    }
}
