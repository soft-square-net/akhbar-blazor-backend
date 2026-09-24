using System.Text.Json;
using FSH.Framework.Core.Storage.File;
using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Buckets.CreateFile.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Enums;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class CreateBucketFileEndpoint
{
    public static RouteHandlerBuilder MapBucketFileCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            //.MapPost("/{bucketId:guid}/folder/{parentFolderId:guid}/CreateFile", async (Guid bucketId, Guid parentFolderId, FileType fileType, IFormFile file, ISender mediator) =>
            .MapPost("/{bucketId:guid}/folder/{parentFolderId:guid}/CreateFile", async (Guid bucketId, Guid parentFolderId, FileType fileType, IFormFile file,string description, string extension, string metaTags, ISender mediator) =>
            {
                if (file == null || file.Length == 0)
                    return Results.BadRequest("Invalid file stream payload.");

                // if (bucketId != request.BucketId || parentFolderId != request.ParentFolderId) return Results.BadRequest();
                // string extension = file.FileName.Split(".").Last();
                Dictionary<string, string> meta = JsonSerializer.Deserialize<Dictionary<string,string>>(metaTags);

                var request = new CreateBucketFileCommand(bucketId, parentFolderId, fileType, file.FileName, description, extension, file.ContentType, file.Length, file.OpenReadStream(), meta);
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateBucketFileEndpoint))
            .WithSummary("creates a bucket File")
            .WithDescription("creates a bucket File")
            .Produces<CreateBucketFileResponse>()
            .RequirePermission("Permissions.Files.Create")
            .DisableAntiforgery()
            .MapToApiVersion(1);
    }

}
