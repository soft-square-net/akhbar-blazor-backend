using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Buckets.Search.v1;

public class SearchBucketsRequest : PaginationFilter, IRequest<PagedList<SingleBucketResponse>>
{
    public Guid StorageAccountId { get; set; }
    public string? BucketName { get; set; }
    public string? Region { get; set; }
    public string? Description { get; set; }

    public SearchBucketsRequest(PaginationFilter Filter)
    {

    }
}
