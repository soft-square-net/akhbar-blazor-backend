using System.ComponentModel;
using FSH.Starter.WebApi.Document.Domain;

namespace FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
public sealed record BucketResponse(Guid? Id, StorageAccount StorageAccount, string Name, string ResourceName, string Region, string? Description, long Size,long MaxSize, DateTimeOffset Created);
