using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Starter.WebApi.Document.Domain;

namespace FSH.Starter.WebApi.Document.Application.Buckets.Search.v1;
public class SearchBucketSpecs : EntitiesByPaginationFilterSpec<Bucket, SingleBucketResponse>
{
    public SearchBucketSpecs(SearchBucketsRequest command)
        : base(command) =>
        Query
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Where(b => b.Name.Contains(command.Keyword), !string.IsNullOrEmpty(command.Keyword));
}
