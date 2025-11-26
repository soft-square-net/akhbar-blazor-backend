using FSH.Framework.Core.Paging;
using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Documents.Get.v1;
using FSH.Starter.WebApi.Document.Application.Documents.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Documents.v1;
public static class SearchDocumentsEndpoint
{
    internal static RouteHandlerBuilder MapGetDocumentListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchDocumentsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchDocumentsEndpoint))
            .WithSummary("Gets a list of documents")
            .WithDescription("Gets a list of documents with pagination and filtering support")
            .Produces<PagedList<DocumentResponse>>()
            .RequirePermission("Permissions.Documents.View")
            .MapToApiVersion(1);
    }
}
