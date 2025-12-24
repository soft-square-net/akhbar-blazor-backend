using FSH.Framework.Core.Domain;
using FSH.Starter.WebApi.Document.Domain.Events;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Domain;
public class File : AuditableEntity
{
    public string Key { get; private set; } = string.Empty; 
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Extension { get; private set; } = string.Empty;
    public string Etag { get; private set; } = string.Empty;
    public string Url { get; private set; } = string.Empty;
    public FileType FileType { get; private set; } = FileType.Other;
    public Guid FolderId { get; private set; }
    public long? Size { get; private set; }
    public bool IsPublic { get; private set; } = true;

    public Folder Folder { get; private set; } = null!;
    private File() { }

    protected File(Guid id, Guid folderId, string key, string name, string extension,string url, FileType fileType, long size, bool isBublic, string? description)
    {
        Id = id;
        FolderId = folderId;
        Key = key;
        Name = name;
        Extension = extension;
        Url = url;
        FileType = fileType;
        Size = size;
        IsPublic = isBublic;
        Description = description;
        QueueDomainEvent(new FileCreated { File = this });
    }

    public static File Create(Folder folder, string key, string name, string extension, string url, FileType fileType, long size, bool isBublic, string? description)
    {
        return new File(Guid.NewGuid(), folder.Id, key, name, extension, url, fileType, size, isBublic, description);
    }

    internal File Update(string? name, string? description)
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

    internal File UpdateUrl(string url)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(url) && !string.Equals(Url, url, StringComparison.OrdinalIgnoreCase))
        {
            Url = url;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new FileUrlUpdated { File = this });
        }
        return this;
    }
    }


