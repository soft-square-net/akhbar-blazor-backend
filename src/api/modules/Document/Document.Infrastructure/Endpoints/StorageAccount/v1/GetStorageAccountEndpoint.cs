using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.StorageAccounts.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.StorageAccounts.v1;
public static class GetStorageAccountEndpoint
{
    internal static RouteHandlerBuilder MapGetStorageAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetStorageAccountRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetStorageAccountEndpoint))
            .WithSummary("gets storage account by id")
            .WithDescription("gets storage account by id")
            .Produces<StorageAccountResponse>()
            .RequirePermission("Permissions.StorageAccounts.View")
            .MapToApiVersion(1);
    }
}
