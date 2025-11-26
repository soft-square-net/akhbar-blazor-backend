using FSH.Framework.Core.Storage.Bucket;
using FSH.Framework.Core.Storage.File;
using Shared.Enums;

namespace FSH.Framework.Core.Storage;
public interface IStorageServiceFactory
{
    public IFileStorageService GetFileStorageService(StorageProvider type);
    public IBucketStorageService GetBucketStorageService(StorageProvider type);
}
