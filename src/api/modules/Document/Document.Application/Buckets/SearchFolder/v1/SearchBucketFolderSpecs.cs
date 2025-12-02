using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.Document.Appication.Buckets.GetFolder.v1;
using FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
using FSH.Starter.WebApi.Document.Domain;

namespace FSH.Starter.WebApi.Document.Application.Buckets.SearchFolder.v1;
public class SearchBucketFolderSpecs : EntitiesByPaginationFilterSpec<Domain.Folder, GetBucketFolderResponse>
{
    public SearchBucketFolderSpecs(SearchBucketFolderRequest command)
        : base(command) =>
        Query
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Where(b => b.Name.Contains(command.Keyword), !string.IsNullOrEmpty(command.Keyword));
}
