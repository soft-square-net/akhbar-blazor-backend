
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.Document.Appication.Buckets.GetFolder.v1;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Buckets.SearchFolder.v1;

public class SearchBucketFolderRequest : PaginationFilter, IRequest<PagedList<GetBucketFolderResponse>>
{
    public Guid BucketId { get; set; }
    public string? Name { get; set; }
    public string? Key { get; set; }
    public string? Description { get; set; }

    public SearchBucketFolderRequest(PaginationFilter filter)
    {
            
    }
}
