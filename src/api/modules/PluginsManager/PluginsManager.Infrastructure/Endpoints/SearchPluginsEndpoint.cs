using FSH.Framework.Core.Paging;
using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.PluginsManager.Application.Plugins.Search;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace PluginsManager.Infrastructure.Endpoints;

public static class SearchPluginsEndpoint
{
    internal static RouteHandlerBuilder MapSearchPluginsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/search", async (ISender mediator, [FromBody] SearchPluginsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchPluginsEndpoint))
            .WithSummary("Gets a list of todo items with paging support")
            .WithDescription("Gets a list of todo items with paging support")
            .Produces<PagedList<SearchPluginsResponse>>()
            .RequirePermission("Permissions.Plugins.View")
            .MapToApiVersion(1);
    }
}
