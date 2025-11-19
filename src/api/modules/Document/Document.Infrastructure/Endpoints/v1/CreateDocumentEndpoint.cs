using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Documents.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.v1;
public static class CreateDocumentEndpoint
{
    internal static RouteHandlerBuilder MapDocumentCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateDocumentCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateDocumentEndpoint))
            .WithSummary("creates a document")
            .WithDescription("creates a document")
            .Produces<CreateDocumentResponse>()
            .RequirePermission("Permissions.Documents.Create")
            .MapToApiVersion(1);
    }

}
