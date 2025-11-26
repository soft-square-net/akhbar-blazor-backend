using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Appication.Buckets.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class CreateBucketEndpoint
{
    public static RouteHandlerBuilder MapBucketCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateBucketCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateBucketEndpoint))
            .WithSummary("creates a bucket")
            .WithDescription("creates a bucket")
            .Produces<CreateBucketResponse>()
            .RequirePermission("Permissions.Buckets.Create")
            .MapToApiVersion(1);
    }

}
