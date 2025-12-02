using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.Document.Appication.Buckets.GetFile.v1;

namespace FSH.Starter.WebApi.Document.Application.Buckets.SearchFiles.v1;
public class SearchBucketFilesSpecs : EntitiesByPaginationFilterSpec<Domain.File, GetBucketFileResponse>
{
    public SearchBucketFilesSpecs(SearchBucketFilesRequest command)
        : base(command) =>
        Query
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Where(b => b.Name.Contains(command.Keyword), !string.IsNullOrEmpty(command.Keyword));
}
