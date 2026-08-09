using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Appication.AccessRules.GetUserAccessRules.v1;
using FSH.Starter.WebApi.Document.Application.AccessRules.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.AccessRules.v1;
public static class GetUserAccessRulesEndpoint
{
    internal static RouteHandlerBuilder MapGetUserAccessRulesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/UserAccessRules/{userId:guid?}", async (Guid? userId, ISender mediator) =>
            {
                var response = await mediator.Send(new GetUserAccessRulesRequest(userId));
                return Results.Ok(response);
            })
            .WithName(nameof(GetUserAccessRulesEndpoint))
            .WithSummary("gets (User & User Roles) Access Rules")
            .WithDescription("gets (User & User Roles) Access Rules")
            .Produces<List<GetUserAccessRulesResponse>?>()
            .RequirePermission("Permissions.AccessRules.View")
            .MapToApiVersion(1);
    }
}
