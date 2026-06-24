using FSH.Framework.Core.Storage.Bucket.Features;

namespace FSH.Framework.Core.Storage.Bucket;
public interface IBucketStorageService
{
    Task<SvcCreateBucketResponse> CreateBucketAsync(SvcCreateBucketCommand command);
    Task<SvcCreateBucketResponse> CreateBucketFolderAsync(SvcCreateBucketFolderCommand command);
    Task<SvcCreateBucketResponse> CreateBucketFileAsync(SvcCreateBucketFileCommand command);
    Task<SvcGetAllBucketsResponse> GetAllBucketsAsync(SvcGetAllBucketsRequest request);
    Task<List<StorageEndpoint>> ListRegionsAsync(SvcListRegionsCommand request);
    Task DeleteBucketsAsync(SvcDeleteBucketsRequest request);
    Task DeleteBucketFolderAsync(SvcDeleteBucketFolderRequest request);
    Task DeleteBucketFileAsync(SvcDeleteBucketFileRequest request);

}
