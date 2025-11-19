
using FSH.Framework.Core.Exceptions;
namespace FSH.Starter.WebApi.Document.Domain.Exceptions;
public sealed class FileNotFoundException : NotFoundException
{
    public FileNotFoundException(Guid id)
        : base($"file with id {id} not found")
    {
    }

    public FileNotFoundException(string exceptionMessage)
        : base(exceptionMessage)
    {
    }
}
