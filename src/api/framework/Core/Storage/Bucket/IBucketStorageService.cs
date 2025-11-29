using FSH.Framework.Core.Storage.Bucket.Features;

namespace FSH.Framework.Core.Storage.Bucket;
public interface IBucketStorageService
{
    Task<SvcCreateBucketResponse> CreateBucketAsync(SvcCreateBucketCommand command);
    Task<GetAllBucketsResponse> GetAllBucketsAsync(GetAllBucketsRequest request);
    Task DeleteBucketsAsync(DeleteBucketsRequest request);

}
