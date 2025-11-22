using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Document.Domain.Events;
public sealed record FolderUpdated : DomainEvent
{
    public Folder? Folder { get; set; }
}
