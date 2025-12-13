using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.Document.Appication.Buckets.GetFile.v1;
using FSH.Starter.WebApi.Document.Domain;

namespace FSH.Starter.WebApi.Document.Application.Buckets.SearchFiles.v1;
public class SearchBucketFilesSpecs : EntitiesByPaginationFilterSpec<Bucket, GetBucketFileResponse>
{
    public SearchBucketFilesSpecs(SearchBucketFilesRequest command)
        : base(command) =>
        Query
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Include(b => b.Folders).ThenInclude(f => f.Files.Where(f => f.Name.Contains(command.Keyword)))
            .Where(b => b.Id == command.BucketId);
            // .Where(b => b.Name.Contains(command.Keyword), !string.IsNullOrEmpty(command.Keyword));
}
