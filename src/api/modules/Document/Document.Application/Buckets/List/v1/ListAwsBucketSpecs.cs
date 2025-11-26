using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.Document.Domain;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.List.v1;
public class ListAwsBucketSpecs : EntitiesByPaginationFilterSpec<Bucket, BucketResponse>
{
    public ListAwsBucketSpecs(ListBucketCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Where(b => b.Name.Contains(command.Keyword), !string.IsNullOrEmpty(command.Keyword));
}
