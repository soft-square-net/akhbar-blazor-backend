using System.Net;
using System.Net.Http.Headers;
using Elsa.Studio;
using Elsa.Studio.Contracts;
using Elsa.Studio.Login.Contracts;

namespace ElsaStudioBlazorWasm;

public class CustomAuthenticationHandler(IRefreshTokenService refreshTokenService, IBlazorServiceAccessor blazorServiceAccessor)
    : DelegatingHandler
{
    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var sp = blazorServiceAccessor.Services;
        var jwtAccessor = sp.GetRequiredService<IJwtAccessor>();
        var accessToken = await jwtAccessor.ReadTokenAsync(TokenNames.AccessToken);
        request.Headers.Authorization = new("CustomBearer", accessToken);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            // Refresh token and retry once.
            var tokens = await refreshTokenService.RefreshTokenAsync(cancellationToken);
            request.Headers.Authorization = new("CustomBearer", tokens.AccessToken);

            // Retry.
            response = await base.SendAsync(request, cancellationToken);
        }

        return response;
    }
}
