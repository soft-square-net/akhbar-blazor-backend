
using FSH.Starter.ElsaWorkflow.Infrastructure.Auth.ApiKey;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.ElsaWorkflow.Infrastructure.Endpoints;

public static class GetApiKeyEnpoints
{
    internal static RouteHandlerBuilder MapGetApiKeyEnpoints(this IEndpointRouteBuilder endpoints )
    {
        return endpoints.MapGet("/", async (IApiKeyStore store) =>
        {
            var apiKey = await store.GetApiKeyAsync("default");

            return Results.Ok("Your Api Key Returned");
        })
        .WithName(nameof(GetApiKeyEnpoints))
        .AllowAnonymous()
        .Produces<string>(StatusCodes.Status200OK)
        .MapToApiVersion(1);
    }
}
