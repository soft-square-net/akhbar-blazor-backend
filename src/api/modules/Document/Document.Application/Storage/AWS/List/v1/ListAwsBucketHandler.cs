using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Application.Documents.Get.v1;
using FSH.Starter.WebApi.Document.Domain;
using FSH.Starter.WebApi.Document.Application.Documents.Search.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Document.Application.Storage.AWS.List.v1;

public sealed class ListAwsBucketHandler( 
    [FromKeyedServices("document:documents")] IReadRepository<Domain.Document> repository)
    : IRequestHandler<ListAwsBucketCommand, PagedList<AwsBucketResponse>>
{
    public async Task<PagedList<AwsBucketResponse>> Handle(ListAwsBucketCommand request, CancellationToken cancellationToken)
{
    ArgumentNullException.ThrowIfNull(request);

    var spec = new ListAwsBucketSpecs(request);

    var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
    var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

    return new PagedList<AwsBucketResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
}
}
