using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Document.Domain.Events;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Domain;
public class Bucket : AuditableEntity, IAggregateRoot
{

    public StorageProvider Provider { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    private Bucket() { }

    private Bucket(Guid id, StorageProvider provider, string name, string? description)
    {
        Id = id;
        Provider = provider;
        Name = name;
        Description = description;
        QueueDomainEvent(new BucketCreated { Bucket = this });
    }

    public static Bucket Create(StorageProvider provider, string name, string? description)
    {
        return new Bucket(Guid.NewGuid(), provider, name, description);
    }

    public Bucket Update(string? name, string? description)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new BucketUpdated { Bucket = this });
        }

        return this;
    }
}
