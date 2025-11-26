using FSH.Framework.Core.Paging;
using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Documents.Get.v1;
using FSH.Starter.WebApi.Document.Application.Documents.Search.v1;
using FSH.Starter.WebApi.Document.Application.StorageAccounts.Get.v1;
using FSH.Starter.WebApi.Document.Application.StorageAccounts.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.StorageAccounts.v1;
public static class SearchStorageAccountsEndpoint
{
    internal static RouteHandlerBuilder MapGetStorageAccountListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchStorageAccountsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchStorageAccountsEndpoint))
            .WithSummary("Gets a list of storage accounts")
            .WithDescription("Gets a list of storage accounts with pagination and filtering support")
            .Produces<PagedList<StorageAccountResponse>>()
            .RequirePermission("Permissions.StorageAccounts.View")
            .MapToApiVersion(1);
    }
}
