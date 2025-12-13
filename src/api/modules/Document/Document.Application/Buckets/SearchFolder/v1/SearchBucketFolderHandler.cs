using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
using FSH.Starter.WebApi.Document.Appication.Buckets.GetFolder.v1;

namespace FSH.Starter.WebApi.Document.Application.Buckets.SearchFolder.v1;
public sealed class SearchBucketFolderHandler(
    [FromKeyedServices("document:buckets")] IReadRepository<Bucket> repository)
    : IRequestHandler<SearchBucketFolderRequest, PagedList<GetBucketFolderResponse>>
{
    public async Task<PagedList<GetBucketFolderResponse>> Handle(SearchBucketFolderRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchBucketFolderSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<GetBucketFolderResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
