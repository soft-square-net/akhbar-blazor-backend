using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Buckets.DownloadFile.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class DownloadBucketFileEndpoint
{
    public static RouteHandlerBuilder MapDownloadBucketFileEndpoint(this IEndpointRouteBuilder endpoints)
    {
        string ContentType = "application/octet-stream";
        return endpoints
            .MapGet("/{id:guid}/File/{fileid:guid}/DownloadFile", async (Guid id, Guid fileid, ISender mediator, HttpContext context) =>
            {
                var response = await mediator.Send(new DownloadBucketFileRequest(id, fileid));
                response.downloadedStream.Position = 0;
                ContentType = response.contentType;
                // if (!string.IsNullOrWhiteSpace(response.contentLanguage))
                // {
                //     context.Response.Headers["Content-Language"] = response.contentLanguage;
                // }
                //
                // if (!string.IsNullOrWhiteSpace(response.contentDisposition))
                // {
                //     context.Response.Headers["Content-Disposition"] = response.contentDisposition;
                // }
                //
                // if (!string.IsNullOrWhiteSpace(response.contentMD5))
                // {
                //     context.Response.Headers["Content-MD5"] = response.contentMD5;
                // }
                //
                // if (response.expires.HasValue)
                // {
                //     context.Response.Headers["Expires"] = response.expires.Value.ToString("R");
                // }

                return Results.File(response.downloadedStream, response.contentType);
            })
            .WithName(nameof(DownloadBucketFileEndpoint))
            .WithSummary("Download bucket File by Id")
            .WithDescription("Download bucket File by Id")
            .Produces<Stream>(StatusCodes.Status200OK, ContentType)
            .RequirePermission("Permissions.Files.View")
            .MapToApiVersion(1);
    }

}
