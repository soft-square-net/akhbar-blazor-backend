using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Buckets.Search.v1;

public class SearchBucketsCommand : PaginationFilter, IRequest<PagedList<BucketResponse>>
{
    public string? Name { get; set; }
    public string? Key { get; set; }
    public string? Description { get; set; }
}
