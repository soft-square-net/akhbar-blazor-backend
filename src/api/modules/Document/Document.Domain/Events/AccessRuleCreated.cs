using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Document.Domain.Events;
public sealed record AccessRuleCreated : DomainEvent
{
    public AccessRule? AccessRule { get; set; }
}
