using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Buckets.SearchFiles.v1;

public class SearchBucketFilesRequest : PaginationFilter, IRequest<PagedList<SearchBucketFileResponse>>
{
    public Guid? BucketId { get; set; }
    public string? Name { get; set; }
    public string? Key { get; set; }
    public string? Description { get; set; }
    public SearchBucketFilesRequest(PaginationFilter filter)
    {
            
    }
}
