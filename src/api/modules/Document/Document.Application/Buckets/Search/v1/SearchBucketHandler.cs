using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
using FSH.Framework.Core.Storage.Bucket.Features;

namespace FSH.Starter.WebApi.Document.Application.Buckets.Search.v1;
public sealed class SearchBucketsHandler(
    [FromKeyedServices("document:buckets")] IReadRepository<Bucket> repository)
    : IRequestHandler<SearchBucketsRequest, PagedList<SingleBucketResponse>>
{
    public async Task<PagedList<SingleBucketResponse>> Handle(SearchBucketsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchBucketSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<SingleBucketResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
