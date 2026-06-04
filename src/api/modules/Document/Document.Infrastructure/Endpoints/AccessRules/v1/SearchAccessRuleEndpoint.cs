using FSH.Framework.Core.Paging;
using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Documents.Get.v1;
using FSH.Starter.WebApi.Document.Application.Documents.Search.v1;
using FSH.Starter.WebApi.Document.Application.AccessRules.Get.v1;
using FSH.Starter.WebApi.Document.Application.AccessRules.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.AccessRules.v1;
public static class SearchAccessRulesEndpoint
{
    internal static RouteHandlerBuilder MapGetAccessRuleListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchAccessRulesCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchAccessRulesEndpoint))
            .WithSummary("Gets a list of storage accounts")
            .WithDescription("Gets a list of storage accounts with pagination and filtering support")
            .Produces<PagedList<AccessRuleResponse>>()
            .RequirePermission("Permissions.AccessRules.View")
            .MapToApiVersion(1);
    }
}
