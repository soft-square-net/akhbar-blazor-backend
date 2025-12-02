using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Buckets.SearchFiles.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class SearchBucketFilesEndpoint
{
    public static RouteHandlerBuilder MapBucketSearchFilesEndpoint(this IEndpointRouteBuilder endpoints)
    {

        return endpoints
            .MapPost("/Search/{id:guid}/Folder/{folderid:guid}/Files", async (ISender mediator, [FromBody] PaginationFilter filter) =>
            {
                var response = await mediator.Send(new SearchBucketFilesRequest(filter));
                return Results.Ok(response);
            })
            .WithName(nameof(SearchBucketFilesEndpoint))
            .WithSummary("Search bucket files with paging support")
            .WithDescription("Search bucket files with paging support")
            .Produces<PagedList<SingleBucketResponse>>()
            .RequirePermission("Permissions.Buckets.View")
            .MapToApiVersion(1);
    }

}
