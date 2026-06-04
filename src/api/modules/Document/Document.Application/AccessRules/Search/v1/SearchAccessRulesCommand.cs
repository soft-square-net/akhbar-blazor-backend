using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.Document.Application.AccessRules.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.AccessRules.Search.v1;

public class SearchAccessRulesCommand : PaginationFilter, IRequest<PagedList<AccessRuleResponse>>
{
    //public string? AccountName { get; set; }
    //public string? Description { get; set; }
}
