using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Document.Domain.Events;
public sealed record FileCreated : DomainEvent
{
    public File? File { get; set; }
}
