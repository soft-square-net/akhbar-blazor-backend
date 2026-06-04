using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.AccessRules.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.AccessRules.v1;
public static class DeleteAccessRuleEndpoint
{
    internal static RouteHandlerBuilder MapAccessRuleDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                await mediator.Send(new DeleteAccessRuleCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(DeleteAccessRuleEndpoint))
            .WithSummary("deletes storage account by id")
            .WithDescription("deletes storage account by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.AccessRules.Delete")
            .MapToApiVersion(1);
    }
}
