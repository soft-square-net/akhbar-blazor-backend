using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.AccessRules.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.AccessRules.v1;
public static class GetAccessRuleEndpoint
{
    internal static RouteHandlerBuilder MapGetAccessRuleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetAccessRuleRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetAccessRuleEndpoint))
            .WithSummary("gets storage account by id")
            .WithDescription("gets storage account by id")
            .Produces<AccessRuleResponse>()
            .RequirePermission("Permissions.AccessRules.View")
            .MapToApiVersion(1);
    }
}
