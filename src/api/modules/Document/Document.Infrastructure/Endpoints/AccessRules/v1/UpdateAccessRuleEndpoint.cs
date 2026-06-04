using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Documents.Update.v1;
using FSH.Starter.WebApi.Document.Application.AccessRules.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.AccessRules.v1;
public static class UpdateAccessRuleEndpoint
{
    internal static RouteHandlerBuilder MapAccessRuleUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateAccessRuleCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateAccessRuleEndpoint))
            .WithSummary("update a storage account")
            .WithDescription("update a storage account")
            .Produces<UpdateAccessRuleResponse>()
            .RequirePermission("Permissions.AccessRules.Update")
            .MapToApiVersion(1);
    }
}
