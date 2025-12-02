using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Appication.Buckets.DeleteFolder.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class DeleteBucketFolderEndpoint
{
    internal static RouteHandlerBuilder MapBucketDeleteFolderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}/Folder/{folderid:guid}/", async (Guid id,Guid folderid , ISender mediator) =>
            {
                await mediator.Send(new DeleteBucketFolderCommand(id,folderid));
                return Results.NoContent();
            })
            .WithName(nameof(DeleteBucketFolderEndpoint))
            .WithSummary("deletes bucket folder by id")
            .WithDescription("deletes bucket folder by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Bucket.Delete")
            .MapToApiVersion(1);
    }
}
