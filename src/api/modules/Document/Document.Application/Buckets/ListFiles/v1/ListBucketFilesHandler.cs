using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Framework.Core.Storage.File;
using FSH.Starter.WebApi.Document.Application.Buckets.ListFiles.v1;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.ListFiles.v1;
public sealed class ListBucketFileHandler(
    ILogger<ListBucketFileHandler> logger, IStorageServiceFactory serviceFactory,
    [FromKeyedServices("document:buckets")] IRepository<Bucket> repository
    ) : IRequestHandler<ListBucketFilesRequest, PagedList<SingleBucketResponse>>
{
   
    public async Task<PagedList<SingleBucketResponse>> Handle(ListBucketFilesRequest request, CancellationToken cancellationToken)
    {
        var Bucket = repository.GetByIdAsync(request.BucketId, cancellationToken);
        var service = serviceFactory.GetFileStorageService(StorageProvider.AmazonS3);
        ArgumentNullException.ThrowIfNull(request);

        var spec = new ListBucketFilesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<SingleBucketResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
