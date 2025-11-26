using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Appication.StorageAccounts.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.StorageAccounts.v1;
public static class CreateStorageAccountEndpoint
{
    internal static RouteHandlerBuilder MapStorageAccountCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateStorageAccountCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateStorageAccountEndpoint))
            .WithSummary("creates a storage account")
            .WithDescription("creates a storage account")
            .Produces<CreateStorageAccountResponse>()
            .RequirePermission("Permissions.StorageAccounts.Create")
            .MapToApiVersion(1);
    }

}
