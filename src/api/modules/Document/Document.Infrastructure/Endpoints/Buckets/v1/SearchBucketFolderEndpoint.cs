using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Appication.Buckets.Create.v1;
using FSH.Starter.WebApi.Document.Application.Buckets.SearchFolder.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class SearchBucketFolderEndpoint
{
    public static RouteHandlerBuilder MapBucketSearchFolderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/Search/{id:guid}/Folder/{folderid:guid}/Folder", async (SearchBucketFolderRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchBucketFolderEndpoint))
            .WithSummary("creates a bucket")
            .WithDescription("creates a bucket")
            .Produces<PagedList<SingleBucketResponse>>()
            .RequirePermission("Permissions.Buckets.Create")
            .MapToApiVersion(1);
    }

}
