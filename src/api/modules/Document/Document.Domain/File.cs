using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Framework.Core.Storage.File;
using FSH.Starter.WebApi.Document.Domain.Events;

namespace FSH.Starter.WebApi.Document.Domain;
public class File : AuditableEntity, IAggregateRoot
{

    public string Key { get; private set; } = string.Empty; 
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Extension { get; private set; }
    public FileType? FileType { get; private set; }
    public Folder? Folder { get; private set; }
    public long? Size { get; private set; }
    public bool IsPublic { get; private set; } = true;

    private File() { }

    private File(Guid id, Folder folder, string name, string extension, FileType fileType, long size, bool isBublic, string? description)
    {
        Id = id;
        Folder = folder;
        Name = name;
        Extension = extension;
        FileType = fileType;
        Size = size;
        IsPublic = isBublic;
        Description = description;
        QueueDomainEvent(new FileCreated { File = this });
    }

    public static File Create(Folder folder, string name, string extension, FileType fileType, long size, bool isBublic, string? description)
    {
        return new File(Guid.NewGuid(),folder, name, extension, fileType, size, isBublic, description);
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


