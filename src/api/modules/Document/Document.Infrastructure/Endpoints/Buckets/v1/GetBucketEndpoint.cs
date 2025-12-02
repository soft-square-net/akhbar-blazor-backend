using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class GetBucketEndpoint
{
    public static RouteHandlerBuilder MapBucketGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetBucketRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetBucketEndpoint))
            .WithSummary("Get bucket by Id")
            .WithDescription("Get bucket by Id")
            .Produces<BucketResponse>()
            .RequirePermission("Permissions.Buckets.View")
            .MapToApiVersion(1);
    }

}
