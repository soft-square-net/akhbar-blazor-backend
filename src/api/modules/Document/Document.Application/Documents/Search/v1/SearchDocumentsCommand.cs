using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.Document.Application.Documents.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Documents.Search.v1;

public class SearchDocumentsCommand : PaginationFilter, IRequest<PagedList<DocumentResponse>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
