using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Documents.Update.v1;
using FSH.Starter.WebApi.Document.Application.StorageAccounts.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.StorageAccounts.v1;
public static class UpdateStorageAccountEndpoint
{
    internal static RouteHandlerBuilder MapStorageAccountUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateStorageAccountCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateStorageAccountEndpoint))
            .WithSummary("update a storage account")
            .WithDescription("update a storage account")
            .Produces<UpdateDocumentResponse>()
            .RequirePermission("Permissions.StorageAccount.Update")
            .MapToApiVersion(1);
    }
}
