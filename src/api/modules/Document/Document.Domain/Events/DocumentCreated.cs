using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Document.Domain.Events;
public sealed record DocumentCreated : DomainEvent
{
    public Document? Document { get; set; }
}
