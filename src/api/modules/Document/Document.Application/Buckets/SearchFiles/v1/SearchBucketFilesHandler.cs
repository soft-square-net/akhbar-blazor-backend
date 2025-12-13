using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
using FSH.Starter.WebApi.Document.Appication.Buckets.GetFile.v1;

namespace FSH.Starter.WebApi.Document.Application.Buckets.SearchFiles.v1;
public sealed class SearchBucketFilesHandler(
    [FromKeyedServices("document:buckets")] IReadRepository<Bucket> repository)
    : IRequestHandler<SearchBucketFilesRequest, PagedList<GetBucketFileResponse>>
{
    public async Task<PagedList<GetBucketFileResponse>> Handle(SearchBucketFilesRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchBucketFilesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<GetBucketFileResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
