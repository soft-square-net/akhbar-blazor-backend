using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Buckets.Create.v1;
using FSH.Starter.WebApi.Document.Application.Buckets.RenameFolder.v1;
using FSH.Starter.WebApi.Document.Application.Buckets.UpdateFolder.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class RenameBucketFolderEndpoint
{
    public static RouteHandlerBuilder MapBucketRenameFolderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/Folder/{folderid:guid}/rename", async (Guid id,Guid folderid,RenameBucketFolderCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(RenameBucketFolderEndpoint))
            .WithSummary("rename bucket folder by id")
            .WithDescription("rename bucket folder by id")
            .Produces<RenameBucketFolderResponse>()
            .RequirePermission("Permissions.Folder.update")
            .MapToApiVersion(1);
    }

}
