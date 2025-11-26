using FSH.Framework.Core.Storage.Bucket.Features;

namespace FSH.Framework.Core.Storage.Bucket;
public interface IBucketStorageService
{
    Task<CreateBucketResponse> CreateBucketAsync(CreateBucketCommand command);
    Task<GetAllBucketsResponse> GetAllBucketsAsync(GetAllBucketsRequest request);
    Task DeleteBucketsAsync(DeleteBucketsRequest request);

}
