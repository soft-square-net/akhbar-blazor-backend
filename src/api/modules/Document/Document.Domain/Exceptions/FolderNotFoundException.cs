
using FSH.Framework.Core.Exceptions;
namespace FSH.Starter.WebApi.Document.Domain.Exceptions;
public sealed class FolderNotFoundException : NotFoundException
{
    public FolderNotFoundException(Guid id)
        : base($"folder with id {id} not found")
    {
    }

    public FolderNotFoundException(string exceptionMessage)
        : base(exceptionMessage)
    {
    }
}
