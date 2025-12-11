
using System.ComponentModel;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.Create.v1;
public sealed record CreateBucketCommand(
    Guid StorageAccountId,
    [property: DefaultValue("Sample Bucket Name")] string BucketName,
    string? Region,

    [property: DefaultValue("Descriptive Description")] string? Description,
    IDictionary<string, string>? Tags
    //[property: Obsolete("AccessKey and SecretKey are obsolete. Use StorageAccount credentials instead.")]
    //string AccessKey,
    //[property: Obsolete("AccessKey and SecretKey are obsolete. Use StorageAccount credentials instead.")]
    //string SecretKey
) : IRequest<CreateBucketResponse>;
