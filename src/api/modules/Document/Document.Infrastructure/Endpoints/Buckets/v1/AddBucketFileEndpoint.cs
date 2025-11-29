using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Appication.Buckets.AddFile.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class AddBucketFileEndpoint
{
    public static RouteHandlerBuilder MapAddBucketFileEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{bucketId:guid}/folder/{parentFolderId:guid}/AddFile", async (Guid bucketId, Guid parentFolderId, AddBucketFileCommand request, ISender mediator) =>
            {
                if (bucketId != request.BucketId || parentFolderId != request.ParentFolderId) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(AddBucketFileEndpoint))
            .WithSummary("creates a bucket File")
            .WithDescription("creates a bucket File")
            .Produces<AddBucketFileResponse>()
            .RequirePermission("Permissions.Buckets.Create")
            .MapToApiVersion(1);
    }

}
