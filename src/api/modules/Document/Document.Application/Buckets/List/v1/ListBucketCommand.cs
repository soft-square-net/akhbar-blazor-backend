using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.List.v1;
public class ListBucketCommand : PaginationFilter, IRequest<PagedList<BucketResponse>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
