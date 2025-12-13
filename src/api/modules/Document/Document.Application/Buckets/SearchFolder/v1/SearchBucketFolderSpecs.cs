using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.Document.Appication.Buckets.GetFolder.v1;
using FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
using FSH.Starter.WebApi.Document.Domain;

namespace FSH.Starter.WebApi.Document.Application.Buckets.SearchFolder.v1;
public class SearchBucketFolderSpecs : EntitiesByPaginationFilterSpec<Bucket, GetBucketFolderResponse>
{
    public SearchBucketFolderSpecs(SearchBucketFolderRequest command)
        : base(command) =>
        Query
            .OrderBy(c => c.Folders, !command.HasOrderBy())
            .Include(b => b.Folders.Where(f => f.Name.Contains(command.Keyword)))
            .Where(b => b.Id == command.BucketId , !string.IsNullOrEmpty(command.Keyword));
}
