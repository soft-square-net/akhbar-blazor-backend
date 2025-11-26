using static Amazon.Runtime.RefreshingAWSCredentials;

namespace FSH.Starter.WebApi.Document.Infrastructure.Services.Auth;
public interface IExternalRefreshingAWSWithBasicCredentials
{
    public bool UpdateCredentials(string accessKey, string secretKey);
    // protected CredentialsRefreshState GenerateNewCredentials();
    // protected Task<CredentialsRefreshState> GenerateNewCredentialsAsync();

}
