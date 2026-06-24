using FSH.Framework.Core.Paging;
using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Buckets.ListRegions.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class ListRegionsEndpoint
{
    public static RouteHandlerBuilder MapRegionListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/ListRegions", async (ISender mediator, [FromBody] ListRegionsRequest request) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(ListRegionsEndpoint))
            .WithSummary("Gets a list of Regions with paging support")
            .WithDescription("Gets a list of Regions items with paging support")
            .Produces<PagedList<RegionResponse>>()
            .RequirePermission("Permissions.Buckets.View")
            .MapToApiVersion(1);
    }

}
