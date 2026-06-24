using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Buckets.ListRegions.v1;

public class ListRegionsRequest : PaginationFilter, IRequest<PagedList<RegionResponse>>
{
    public Guid StorageAccountId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } =string.Empty;
}
