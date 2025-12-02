using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Appication.Buckets.CreateFolder.v1;
using FSH.Starter.WebApi.Document.Appication.Buckets.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class CreatePucketFolderEndpoint
{
    public static RouteHandlerBuilder MapPucketFolderCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{bucketId:guid}/folder/{parentFolderId:guid}/CreateFolder", async (Guid bucketId, Guid parentFolderId, CreateBucketFolderCommand request, ISender mediator) =>
            {
                if (bucketId != request.BucketId || parentFolderId != request.ParentFolderId) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreatePucketFolderEndpoint))
            .WithSummary("creates a bucket Folder")
            .WithDescription("creates a bucket Folder")
            .Produces<CreateBucketFolderResponse>()
            .RequirePermission("Permissions.Buckets.Create")
            .MapToApiVersion(1);
    }

}
