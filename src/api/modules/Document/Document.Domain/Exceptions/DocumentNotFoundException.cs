
using FSH.Framework.Core.Exceptions;
namespace FSH.Starter.WebApi.Document.Domain.Exceptions;
public sealed class DocumentNotFoundException : NotFoundException
{
    public DocumentNotFoundException(Guid id)
        : base($"document with id {id} not found")
    {
    }

    public DocumentNotFoundException(string exceptionMessage)
        : base(exceptionMessage)
    {
    }
}
