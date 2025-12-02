using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.Document.Appication.Buckets.GetFile.v1;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Buckets.SearchFiles.v1;

public class SearchBucketFilesRequest : PaginationFilter, IRequest<PagedList<GetBucketFileResponse>>
{
    public string? Name { get; set; }
    public string? Key { get; set; }
    public string? Description { get; set; }
    public SearchBucketFilesRequest(PaginationFilter filter)
    {
            
    }
}
