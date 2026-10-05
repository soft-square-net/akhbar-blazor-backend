using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Keyword.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Keyword.Application.Documents.FacetedSearch.v1;

public sealed class DocumentFacetedSearchHandler (
      [FromKeyedServices("keyword:Documents")] IReadRepository<Document> repository) 
 : IRequestHandler<DocumentFacetedSearchCommand, PagedList<DocumentFacetedSearchResponse>>
{
    public async Task<PagedList<DocumentFacetedSearchResponse>> Handle(DocumentFacetedSearchCommand request, CancellationToken cancellationToken)
    {
        var spec = new DocumentFacetedSearchSpec(request);

        //// Uses SpecificationEvaluator from Ardalis.Specification.EntityFrameworkCore
        //List<DocumentFacetedSearchResponse> results = await SpecificationEvaluator.Default
        //    .GetQuery(_dbContext.Documents.AsQueryable(), spec)
        //    .ToListAsync();

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<DocumentFacetedSearchResponse>(items, request!.PageNumber, request!.PageSize, totalCount);

    }
}
