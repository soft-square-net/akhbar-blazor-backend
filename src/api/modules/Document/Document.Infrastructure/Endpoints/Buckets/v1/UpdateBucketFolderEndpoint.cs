using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Appication.Buckets.Create.v1;
using FSH.Starter.WebApi.Document.Appication.Buckets.UpdateFolder.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class UpdateBucketFolderEndpoint
{
    public static RouteHandlerBuilder MapBucketUpdateFolderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/Folder/{folderid:guid}/", async (UpdateBucketFolderCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateBucketFolderEndpoint))
            .WithSummary("update bucket folder by id")
            .WithDescription("update bucket folder by id")
            .Produces<UpdateBucketFolderResponse>()
            .RequirePermission("Permissions.Buckets.update")
            .MapToApiVersion(1);
    }

}
