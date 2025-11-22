using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Document.Domain.Events;
public sealed record BucketCreated : DomainEvent
{
    public Bucket? Bucket { get; set; }
}
