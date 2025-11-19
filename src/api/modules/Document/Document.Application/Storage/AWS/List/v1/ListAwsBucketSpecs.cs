using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.Document.Application.Documents.Get.v1;
using FSH.Starter.WebApi.Document.Application.Documents.Search.v1;

namespace FSH.Starter.WebApi.Document.Application.Storage.AWS.List.v1;
public class ListAwsBucketSpecs : EntitiesByPaginationFilterSpec<FSH.Starter.WebApi.Document.Domain.Document, AwsBucketResponse>
{
    public ListAwsBucketSpecs(ListAwsBucketCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Where(b => b.Name.Contains(command.Keyword), !string.IsNullOrEmpty(command.Keyword));
}
