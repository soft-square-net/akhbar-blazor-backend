using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Buckets.Search.v1;

public class SearchBucketsRequest : PaginationFilter, IRequest<PagedList<BucketResponse>>
{
    // public Guid StorageAccountId { get; set; }
    public string BucketName { get; set; } = string.Empty;
    // public string? Region { get; set; }
    public string Description { get; set; } =string.Empty;

    //public SearchBucketsRequest(PaginationFilter Filter )
    //{

    //}
}
