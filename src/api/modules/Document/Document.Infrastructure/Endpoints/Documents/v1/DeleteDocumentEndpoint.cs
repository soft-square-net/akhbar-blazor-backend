using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Documents.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Documents.v1;
public static class DeleteDocumentEndpoint
{
    internal static RouteHandlerBuilder MapDocumentDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                await mediator.Send(new DeleteDocumentCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(DeleteDocumentEndpoint))
            .WithSummary("deletes document by id")
            .WithDescription("deletes document by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Documents.Delete")
            .MapToApiVersion(1);
    }
}
