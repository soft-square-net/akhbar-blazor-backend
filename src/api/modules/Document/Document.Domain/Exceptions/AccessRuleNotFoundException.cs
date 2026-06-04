
using FSH.Framework.Core.Exceptions;
namespace FSH.Starter.WebApi.Document.Domain.Exceptions;
public sealed class AccessRuleNotFoundException : NotFoundException
{
    public AccessRuleNotFoundException(Guid id)
        : base($"access rule with id {id} not found")
    {
    }

    public AccessRuleNotFoundException(string exceptionMessage)
        : base(exceptionMessage)
    {
    }
}
