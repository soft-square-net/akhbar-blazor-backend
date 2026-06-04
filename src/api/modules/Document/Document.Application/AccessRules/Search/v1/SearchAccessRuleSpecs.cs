using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.Document.Application.AccessRules.Get.v1;
using FSH.Starter.WebApi.Document.Domain;

namespace FSH.Starter.WebApi.Document.Application.AccessRules.Search.v1;
public class SearchAccessRuleSpecs : EntitiesByPaginationFilterSpec<AccessRule, AccessRuleResponse>
{
    public SearchAccessRuleSpecs(SearchAccessRulesCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.Bucket.Name, !command.HasOrderBy())
            .Where(b => b.Description.Contains(command.Keyword) || b.Bucket.Name.Contains(command.Keyword), !string.IsNullOrEmpty(command.Keyword));
}
