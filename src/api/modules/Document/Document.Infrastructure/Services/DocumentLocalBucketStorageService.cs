using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Storage.Bucket;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Starter.WebApi.Document.Infrastructure.Services.Auth;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FSH.Starter.WebApi.Document.Infrastructure.Services;
public class DocumentLocalBucketStorageService : IBucketStorageService
{
    public DocumentLocalBucketStorageService(IExternalRefreshingAWSWithBasicCredentials refreshCeredintials, IConfiguration configuration)
    {
        throw new NotImplementedException();
    }

    public async Task<CreateBucketResponse> CreateBucketAsync(CreateBucketCommand command)
    {
        throw new NotImplementedException();
    }

    public async Task<GetAllBucketsResponse> GetAllBucketsAsync(GetAllBucketsRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteBucketsAsync(DeleteBucketsRequest request)
    {
        throw new NotImplementedException();
    }
}
