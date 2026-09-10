using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Appication.Buckets.DownloadFile.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class DownloadBucketFileEndpoint
{
    public static RouteHandlerBuilder MapDownloadBucketFileEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}/Folder/{folderid:guid}/File{fileid:guid}/", async (Guid id, Guid folderid, Guid fileid,ISender mediator) =>
            {
                var response = await mediator.Send(new DownloadBucketFileRequest(id, folderid, fileid));
                byte[] fileBytes = System.IO.File.ReadAllBytes("path/to/your/file.pdf");
                return Results.File(fileBytes, "application/pdf", "document.pdf");
            })
            .WithName(nameof(DownloadBucketFileEndpoint))
            .WithSummary("Get bucket File by Id")
            .WithDescription("Get bucket File by Id")
            .Produces<DownloadBucketFileResponse>()
            .RequirePermission("Permissions.Files.View")
            .MapToApiVersion(1);
    }

}
