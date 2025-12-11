using Amazon.S3;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.List.v1;
public class ListBucketRequest : PaginationFilter, IRequest<PagedList<BucketDTO>> 
{
    // public Guid StorageAccountId { get; set; }
    //public string? BucketName { get; set; }
    // public string? Region { get; set; } = S3Region.USEast1.Value;
    //public string? Description { get; set; }

    //public ListBucketRequest(PaginationFilter Filter)
    //{
            
    //}
}
