namespace FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
public sealed record BucketResponse(Guid? Id,Guid StorageAccountId, string StorageAccountName, string BucketName,string Region, string? Description, long Size,long MaxSize, DateTimeOffset Created);
