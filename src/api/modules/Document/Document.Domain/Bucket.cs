using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Document.Domain.Events;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Domain;
public class Bucket : AuditableEntity, IAggregateRoot
{
    public StorageAccount StorageAccount { get; private set; }
    public string Region { get; private set; } = string.Empty;
    public string Key { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    // public Folder RootFolder { get; private set; }
    public string? Description { get; private set; }
    public ICollection<Folder>? Folders { get; private set; }

    private Bucket() { }

    private Bucket(Guid id, StorageAccount storageAccount, string region, string key, string name, string? description)
    {
        Id = id;
        StorageAccount = storageAccount;
        Region = region;
        Key = key;
        Name = name;
        Description = description;
        QueueDomainEvent(new BucketCreated { Bucket = this });
    }

    public static Bucket Create(StorageAccount storageAccount, string region, string key, string name, string? description)
    {
        return new Bucket(Guid.NewGuid(), storageAccount, region, key, name, description);
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
