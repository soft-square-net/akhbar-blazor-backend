using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Storage.Bucket;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Starter.WebApi.Document.Infrastructure.Services.Auth;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FSH.Starter.WebApi.Document.Infrastructure.Services.Buckets;
public class DocumentLocalBucketStorageService : IBucketStorageService
{
    public DocumentLocalBucketStorageService(IExternalRefreshingAWSWithBasicCredentials refreshCeredintials, IConfiguration configuration)
    {
        throw new NotImplementedException();
    }

    public async Task<SvcCreateBucketResponse> CreateBucketAsync(SvcCreateBucketCommand command)
    {
        throw new NotImplementedException();
    }

    public async Task<SvcGetAllBucketsResponse> GetAllBucketsAsync(SvcGetAllBucketsRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteBucketsAsync(SvcDeleteBucketsRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<SvcCreateBucketResponse> CreateBucketFolderAsync(SvcCreateBucketFolderCommand command)
    {
        throw new NotImplementedException();
    }

    public Task<SvcCreateBucketResponse> CreateBucketFileAsync(SvcCreateBucketFileCommand command)
    {
        throw new NotImplementedException();
    }


    public Task<List<StorageEndpoint>> ListRegionsAsync(SvcListRegionsCommand request)
    {
        return Task.FromResult(new List<StorageEndpoint>() { StorageEndpoint.Create("local", "Local") });
    }

    public Task DeleteBucketFolderAsync(SvcDeleteBucketFolderRequest request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteBucketFileAsync(SvcDeleteBucketFileRequest request)
    {
        throw new NotImplementedException();
    }
}
