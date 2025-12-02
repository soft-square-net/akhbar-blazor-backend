using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Appication.Buckets.CreateFile.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class CreateBucketFileEndpoint
{
    public static RouteHandlerBuilder MapBucketFileCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{bucketId:guid}/folder/{parentFolderId:guid}/CreateFile", async (Guid bucketId, Guid parentFolderId, CreateBucketFileCommand request, ISender mediator) =>
            {
                if (bucketId != request.BucketId || parentFolderId != request.ParentFolderId) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateBucketFileEndpoint))
            .WithSummary("creates a bucket File")
            .WithDescription("creates a bucket File")
            .Produces<CreateBucketFileResponse>()
            .RequirePermission("Permissions.Buckets.Create")
            .MapToApiVersion(1);
    }

}
