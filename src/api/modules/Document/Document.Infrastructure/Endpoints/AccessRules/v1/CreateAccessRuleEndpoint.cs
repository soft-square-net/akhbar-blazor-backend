using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Appication.AccessRules.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.AccessRules.v1;
public static class CreateAccessRuleEndpoint
{
    internal static RouteHandlerBuilder MapAccessRuleCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateAccessRuleCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateAccessRuleEndpoint))
            .WithSummary("creates a storage account")
            .WithDescription("creates a storage account")
            .Produces<CreateAccessRuleResponse>()
            .RequirePermission("Permissions.AccessRules.Create")
            .MapToApiVersion(1);
    }

}
