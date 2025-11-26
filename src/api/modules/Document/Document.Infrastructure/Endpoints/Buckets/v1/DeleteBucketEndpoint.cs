using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Documents.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class DeleteBucketEndpoint
{
    internal static RouteHandlerBuilder MapBucketDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                await mediator.Send(new DeleteBucketCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(DeleteBucketEndpoint))
            .WithSummary("deletes bucket by id")
            .WithDescription("deletes bucket by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Bucket.Delete")
            .MapToApiVersion(1);
    }
}
