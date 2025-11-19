using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Document.Domain.Events;

namespace FSH.Starter.WebApi.Document.Domain;
public class File : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    private File() { }

    private File(Guid id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
        QueueDomainEvent(new FileCreated { File = this });
    }

    public static File Create(string name, string? description)
    {
        return new File(Guid.NewGuid(), name, description);
    }

    public File Update(string? name, string? description)
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
            QueueDomainEvent(new FileUpdated { File = this });
        }

        return this;
    }
}


