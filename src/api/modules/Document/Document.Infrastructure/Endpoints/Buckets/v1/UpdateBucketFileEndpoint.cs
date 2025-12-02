using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.Document.Appication.Buckets.Create.v1;
using FSH.Starter.WebApi.Document.Appication.Buckets.UpdateFile.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace FSH.Starter.WebApi.Document.Infrastructure.Endpoints.Buckets.v1;
public static class UpdateBucketFileEndpoint
{
    public static RouteHandlerBuilder MapBucketUpdateFileEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/Folder/{folderid:guid}/File/{fileid:guid}", async (UpdateBucketFileCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateBucketFileEndpoint))
            .WithSummary("Update a bucket file")
            .WithDescription("Update a bucket file")
            .Produces<UpdateBucketFileResponse>()
            .RequirePermission("Permissions.Buckets.Update")
            .MapToApiVersion(1);
    }

}
