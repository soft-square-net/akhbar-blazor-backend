
//using FSH.Starter.ElsaWorkflow.Infrastructure.Auth.ApiKey;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;

//namespace FSH.Starter.ElsaWorkflow.Infrastructure.Endpoints;

//record CreateApiKeyRequest(string Owner, List<string> Roles, int? ExpiresInDays);

//public class ApiKeyEnpoints
//{
//    public void MapApiKeyEndpoints(WebApplication app)
//    {
//        app.MapPost("/api/api-keys", async (IApiKeyStore store, CreateApiKeyRequest request) =>
//        {
//            var apiKey = await store.CreateApiKeyAsync(
//                request.Owner,
//                request.Roles,
//                request.ExpiresInDays.HasValue
//                    ? DateTime.UtcNow.AddDays(request.ExpiresInDays.Value)
//                    : null);

//            return Results.Ok(new { apiKey = apiKey.Key, created = apiKey.Created, expires = apiKey.Expires });
//        })
//        .RequireAuthorization();

//        app.MapDelete("/api/api-keys/{key}", async (IApiKeyStore store, string key) =>
//        {
//            await store.RevokeApiKeyAsync(key);
//            return Results.Ok();
//        })
//        .RequireAuthorization();

//    }
//}
