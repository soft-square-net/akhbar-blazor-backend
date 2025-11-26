using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Document.Domain.Events;
public sealed record StorageAccountCreated : DomainEvent
{
    public StorageAccount? StorageAccount { get; set; }
}
