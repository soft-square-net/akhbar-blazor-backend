using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Storage.Bucket.Features;
using MediatR;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.ListFiles.v1;


public class ListBucketFilesRequest : PaginationFilter, IRequest<PagedList<SingleBucketResponse>>
{
    public Guid BucketId { get; set; }
    public string? BucketName { get; set; }
    public string? Region { get; set; }
    public string? Description { get; set; }

    public ListBucketFilesRequest(PaginationFilter Filter)
    {

    }
}

