using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Starter.WebApi.Document.Domain;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.List.v1;
public class ListAwsBucketSpecs : EntitiesByPaginationFilterSpec<Bucket, BucketDTO>
{
    public ListAwsBucketSpecs(ListBucketRequest command)
        : base(command) =>
        Query
            .Include(b => b.StorageAccount)
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Where(b => b.Name.Contains(command.Keyword), !string.IsNullOrEmpty(command.Keyword));
}
