using FSH.Starter.WebApi.Document.Application.Documents.Get.v1;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.v1;
public static class GetDocumentEndpoint
{
    internal static RouteHandlerBuilder MapGetDocumentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetDocumentRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetDocumentEndpoint))
            .WithSummary("gets document by id")
            .WithDescription("gets document by id")
            .Produces<DocumentResponse>()
            .RequirePermission("Permissions.Documents.View")
            .MapToApiVersion(1);
    }
}
