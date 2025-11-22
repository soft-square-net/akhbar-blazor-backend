using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Document.Domain.Events;
public sealed record BucketUpdated : DomainEvent
{
    public Bucket? Bucket { get; set; }
}
