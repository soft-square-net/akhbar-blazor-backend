using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Document.Domain.Events;
public sealed record StorageAccountUpdated : DomainEvent
{
    public StorageAccount? StorageAccount { get; set; }
}
