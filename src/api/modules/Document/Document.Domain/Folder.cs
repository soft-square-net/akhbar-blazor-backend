using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Document.Domain.Events;

namespace FSH.Starter.WebApi.Document.Domain;
public class Folder : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? FullPath { get; private set; }
    public Bucket Bucket { get; private set; }
    public Folder? Parent { get; private set; }
    public ICollection<Folder?>? Children { get; private set; }

    private Folder() { }

    private Folder(Guid id, Bucket bucket, string name, string? description, Folder? parent = null)
    {
        Id = id;
        Bucket = bucket;
        Name = name;
        Description = description;
        Parent = parent;
        QueueDomainEvent(new FolderCreated { Folder = this });
    }

    public static Folder Create(Bucket bucket, string name, string? description, Folder? parent)
    {
        return new Folder(Guid.NewGuid(), bucket, name, description, parent);
    }

    public Folder AddChildFolder(string name, string? description)
    {
        return new Folder(Guid.NewGuid(),this.Bucket, name, description, this);
    }

    public Folder Update(string? name, string? description)
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
            QueueDomainEvent(new FolderUpdated { Folder = this });
        }

        return this;
    }
}
