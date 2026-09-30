using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Application.Buckets.Create.v1;
using FSH.Starter.WebApi.Document.Application.Buckets.RenameFile.v1;
using FSH.Starter.WebApi.Document.Application.Buckets.UpdateFolder.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class RenameBucketFileEndpoint
{
    public static RouteHandlerBuilder MapBucketRenameFileEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/Folder/{folderid:guid}/File/{fileid:guid}/rename", async (Guid id,Guid folderid, Guid fileid, RenameBucketFileCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(RenameBucketFileEndpoint))
            .WithSummary("rename bucket File by id")
            .WithDescription("rename bucket File by id")
            .Produces<RenameBucketFileResponse>()
            .RequirePermission("Permissions.File.update")
            .MapToApiVersion(1);
    }

}
