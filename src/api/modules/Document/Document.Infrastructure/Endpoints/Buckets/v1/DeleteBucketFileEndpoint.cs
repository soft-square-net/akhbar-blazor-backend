using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Appication.Buckets.DeleteFile.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class DeleteBucketFileEndpoint
{
    internal static RouteHandlerBuilder MapBucketDeleteFileEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}/Folder/{folderid:guid}/File/{fileid:guid}", async (Guid id, Guid folderid, Guid fileid, ISender mediator) =>
            {
                await mediator.Send(new DeleteBucketFileCommand(id,folderid,fileid));
                return Results.NoContent();
            })
            .WithName(nameof(DeleteBucketFileEndpoint))
            .WithSummary("deletes bucket by id")
            .WithDescription("deletes bucket by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Bucket.Delete")
            .MapToApiVersion(1);
    }
}
