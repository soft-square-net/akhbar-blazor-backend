using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Buckets.ListFolder.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class ListBucketFolderEndpoint
{
    public static RouteHandlerBuilder MapBucketListFolderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/List/{id:guid}/Folder/{folderid:guid}/Folders", async (ISender mediator, [FromBody] PaginationFilter filter) =>
            {
                var response = await mediator.Send(new ListBucketFolderRequest(filter));
                return Results.Ok(response);
            })
            .WithName(nameof(ListBucketFolderEndpoint))
            .WithSummary("Gets a list of bucket items with paging support")
            .WithDescription("Gets a list of bucket items with paging support")
            .Produces<PagedList<SingleBucketResponse>>()
            .RequirePermission("Permissions.Buckets.View")
            .MapToApiVersion(1);
    }

}
