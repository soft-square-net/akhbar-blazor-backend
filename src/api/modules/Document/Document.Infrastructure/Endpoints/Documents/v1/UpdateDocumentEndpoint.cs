using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Documents.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Documents.v1;
public static class UpdateDocumentEndpoint
{
    internal static RouteHandlerBuilder MapDocumentUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateDocumentCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateDocumentEndpoint))
            .WithSummary("update a document")
            .WithDescription("update a document")
            .Produces<UpdateDocumentResponse>()
            .RequirePermission("Permissions.Documents.Update")
            .MapToApiVersion(1);
    }
}
