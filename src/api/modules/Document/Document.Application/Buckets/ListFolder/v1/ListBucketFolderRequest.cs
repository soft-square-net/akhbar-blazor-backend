using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Storage.Bucket.Features;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Buckets.ListFolder.v1;

public class ListBucketFolderRequest : PaginationFilter, IRequest<PagedList<SingleBucketResponse>>
{
    public Guid BucketId { get; set; }
    public Guid FolderId { get; set; }
    public string? BucketName { get; set; }
    public string? Region { get; set; }
    public string? Description { get; set; }

    public ListBucketFolderRequest(PaginationFilter Filter)
    {

    }
}
