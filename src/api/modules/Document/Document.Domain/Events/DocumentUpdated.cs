using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Document.Domain.Events;
public sealed record DocumentUpdated : DomainEvent
{
    public Document? Document { get; set; }
}
