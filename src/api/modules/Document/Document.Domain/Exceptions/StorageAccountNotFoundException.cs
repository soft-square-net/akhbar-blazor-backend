
using FSH.Framework.Core.Exceptions;
namespace FSH.Starter.WebApi.Document.Domain.Exceptions;
public sealed class StorageAccountNotFoundException : NotFoundException
{
    public StorageAccountNotFoundException(Guid id)
        : base($"storage account with id {id} not found")
    {
    }

    public StorageAccountNotFoundException(string exceptionMessage)
        : base(exceptionMessage)
    {
    }
}
