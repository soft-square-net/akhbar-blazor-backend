
using FSH.Framework.Core.Exceptions;
namespace FSH.Starter.WebApi.Document.Domain.Exceptions;
public sealed class BucketNotFoundException : NotFoundException
{
    public BucketNotFoundException(Guid id)
        : base($"Bucket with id {id} not found")
    {
    }

    public BucketNotFoundException(string exceptionMessage)
        : base(exceptionMessage)
    {
    }
}
