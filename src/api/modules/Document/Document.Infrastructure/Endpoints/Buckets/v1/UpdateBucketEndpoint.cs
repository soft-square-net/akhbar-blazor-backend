using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Appication.Buckets.Create.v1;
using FSH.Starter.WebApi.Document.Application.Buckets.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class UpdateBucketEndpoint
{
    public static RouteHandlerBuilder MapBucketUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateBucketCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateBucketEndpoint))
            .WithSummary("creates a bucket")
            .WithDescription("creates a bucket")
            .Produces<UpdateBucketResponse>()
            .RequirePermission("Permissions.Buckets.Update")
            .MapToApiVersion(1);
    }

}
