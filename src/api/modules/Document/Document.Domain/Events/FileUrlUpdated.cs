using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Document.Domain.Events;
public sealed record FileUrlUpdated : DomainEvent
{
    public File? File { get; set; }
}
