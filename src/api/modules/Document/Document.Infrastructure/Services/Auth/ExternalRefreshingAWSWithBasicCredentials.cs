
using System;
using System.Threading.Tasks;
using Amazon.Runtime;

namespace FSH.Starter.WebApi.Document.Infrastructure.Services.Auth;

// Define a class that inherits from RefreshingAWSCredentials
public class ExternalRefreshingAWSWithBasicCredentials : RefreshingAWSCredentials, IExternalRefreshingAWSWithBasicCredentials
{
    // In a real scenario, these would be fetched securely from an external source 
    // (e.g., AWS Secrets Manager, a database, or an internal service)
    private string _accessKey = string.Empty;
    private string _secretKey = string.Empty;

    public ExternalRefreshingAWSWithBasicCredentials() { }
    public ExternalRefreshingAWSWithBasicCredentials(string accessKey, string secretKey, string region)
    {
        _accessKey = accessKey;
        _secretKey = secretKey;
    }

    public bool UpdateCredentials(string accessKey, string secretKey)
    {
        bool isUpdated = false;
        if (!string.Equals(_accessKey, accessKey, StringComparison.Ordinal))
        {
            _accessKey = accessKey;
            isUpdated = true;
        }
        if (!string.Equals(_secretKey, secretKey, StringComparison.Ordinal))
        {
            _secretKey = secretKey;
            isUpdated = true;
        }

        if(isUpdated) ClearCredentials();
        return isUpdated;
    }
    protected override async Task<CredentialsRefreshState> GenerateNewCredentialsAsync()
    {
        var newCredentials = new ImmutableCredentials(_accessKey, _secretKey, null);
        var expiration = DateTime.UtcNow.AddMinutes(15);
        return new CredentialsRefreshState(newCredentials, expiration);
    }

    protected override CredentialsRefreshState GenerateNewCredentials()
    {
        var newCredentials = new ImmutableCredentials(_accessKey, _secretKey,null);
        var expiration = DateTime.UtcNow.AddMinutes(15);
        return new CredentialsRefreshState(newCredentials, expiration);
    }


}
