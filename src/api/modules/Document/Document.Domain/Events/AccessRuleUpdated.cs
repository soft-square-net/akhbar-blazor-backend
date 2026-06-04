using System.Security.AccessControl;
using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Document.Domain.Events;
public sealed record AccessRuleUpdated : DomainEvent
{
    public AccessRule? AccessRule { get; set; }
}
