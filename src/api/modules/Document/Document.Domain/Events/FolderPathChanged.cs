using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Document.Domain.Events;
public sealed record FolderPathChanged : DomainEvent
{
    public Folder? Folder { get; set; }
}
