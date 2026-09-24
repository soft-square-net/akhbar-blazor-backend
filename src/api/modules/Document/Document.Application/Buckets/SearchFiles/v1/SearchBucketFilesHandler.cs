using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Domain;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Document.Application.Buckets.SearchFiles.v1;
public sealed class SearchBucketFilesHandler(
    [FromKeyedServices("document:buckets")] IReadRepository<Bucket> repository)
    : IRequestHandler<SearchBucketFilesRequest, PagedList<SearchBucketFileResponse>>
{
    public async Task<PagedList<SearchBucketFileResponse>> Handle(SearchBucketFilesRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchBucketFilesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<SearchBucketFileResponse>(items.Adapt<List<SearchBucketFileResponse>>(), request!.PageNumber, request!.PageSize, totalCount);
    }
}
