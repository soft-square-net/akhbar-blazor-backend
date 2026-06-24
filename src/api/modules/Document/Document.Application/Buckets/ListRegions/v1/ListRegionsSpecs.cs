using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
using FSH.Starter.WebApi.Document.Domain;

namespace FSH.Starter.WebApi.Document.Application.Buckets.ListRegions.v1;
public class ListRegionsSpecs : EntitiesByPaginationFilterSpec<Bucket, RegionResponse>
{
    public ListRegionsSpecs(ListRegionsRequest command)
        : base(command) =>
        Query
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Where(b => b.Name.Contains(command.Keyword!), !string.IsNullOrEmpty(command.Keyword))
            .Where(b => (b.Description!.Contains(command.Keyword!)), !string.IsNullOrEmpty(command.Keyword));
}
