using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Application.Documents.Get.v1;
using FSH.Starter.WebApi.Document.Domain;
using FSH.Starter.WebApi.Document.Application.Documents.Search.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Document.Application.Documents.Search.v1;
public sealed class SearchDocumentsHandler(
    [FromKeyedServices("document:documents")] IReadRepository<Domain.Document> repository)
    : IRequestHandler<SearchDocumentsCommand, PagedList<DocumentResponse>>
{
    public async Task<PagedList<DocumentResponse>> Handle(SearchDocumentsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchDocumentSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<DocumentResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
