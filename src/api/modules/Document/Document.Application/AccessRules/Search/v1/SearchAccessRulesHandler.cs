using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Application.AccessRules.Get.v1;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Document.Application.AccessRules.Search.v1;
public sealed class SearchAccessRulesHandler(
    [FromKeyedServices("document:access-rules")] IReadRepository<AccessRule> repository)
    : IRequestHandler<SearchAccessRulesCommand, PagedList<AccessRuleResponse>>
{
    public async Task<PagedList<AccessRuleResponse>> Handle(SearchAccessRulesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchAccessRuleSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<AccessRuleResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
