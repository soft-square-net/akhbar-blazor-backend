
using FSH.Starter.WebApi.Document.Domain;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.List.v1;
public sealed record BucketDTO(Guid? Id, Guid StorageAccountId, string Name, string ResourceName, string Description);
