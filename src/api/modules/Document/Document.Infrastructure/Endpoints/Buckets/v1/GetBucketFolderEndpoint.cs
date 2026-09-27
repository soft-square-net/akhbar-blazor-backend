using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Buckets.GetFolder.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class GetBucketFolderEndpoint
{
    public static RouteHandlerBuilder MapBucketGetFolderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}/Folder/{folderid:guid?}/GetFolder", async (Guid id,Guid? folderid = null , ISender mediator = default) =>
            {
                var response = await mediator.Send(new GetBucketFolderRequest(id, folderid));
                return Results.Ok(response);
            })
            .WithName(nameof(GetBucketFolderEndpoint))
            .WithSummary("Get bucket folder by id")
            .WithDescription("Get bucket folder by id")
            .Produces<GetBucketFolderResponse>()
            .RequirePermission("Permissions.Folders.View")
            .MapToApiVersion(1);
    }

}
