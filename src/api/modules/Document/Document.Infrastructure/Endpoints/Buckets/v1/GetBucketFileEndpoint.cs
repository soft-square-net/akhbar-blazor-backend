using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Appication.Buckets.GetFile.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class GetBucketFileEndpoint
{
    public static RouteHandlerBuilder MapBucketGetFileEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}/Folder/{folderid:guid}/File{fileid:guid}/", async (Guid id, Guid folderid, Guid fileid,ISender mediator) =>
            {
                var response = await mediator.Send(new GetBucketFileRequest(id,folderid,fileid));
                return Results.Ok(response);
            })
            .WithName(nameof(GetBucketFileEndpoint))
            .WithSummary("Get bucket File by Id")
            .WithDescription("Get bucket File by Id")
            .Produces<GetBucketFileResponse>()
            .RequirePermission("Permissions.Buckets.View")
            .MapToApiVersion(1);
    }

}
