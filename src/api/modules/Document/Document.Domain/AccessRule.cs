
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Document.Domain.Events;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Domain;

public class AccessRule : AuditableEntity, IAggregateRoot
{
    public StorageAccount StorageAccount { get; private set; }
    public Guid ResourceOwnerId { get; private set; }
    public ResourceOwnerType ResourceOwnerType { get; private set; }
    public bool IsEnabled { get; private set; } = true;
    public bool Read { get; private set; } = true;
    public bool Write { get; private set; } = false;
    public bool Execute { get; private set; } = false;
    public string Description { get; private set; } = string.Empty;

    public Bucket Bucket { get; private set; }
    public string RootPath { get; private set; } = string.Empty;



    private AccessRule() { }

    private AccessRule(Guid id, StorageAccount storageAccount, ResourceOwnerType resourceOwnerType, Guid resourceOwnerId, Bucket bucket, string rootPath = "", bool enabled = true, bool read = true, bool write = false, bool execute = false, string? description = "")
    {
        Id = id;
        StorageAccount = storageAccount;
        ResourceOwnerId = resourceOwnerId;
        ResourceOwnerType = resourceOwnerType;
        IsEnabled = enabled;
        Read = read; Write = write; Execute = execute;
        Description = description ??= string.Empty;
        Bucket = bucket;
        RootPath = rootPath;
        QueueDomainEvent(new AccessRuleCreated { AccessRule = this });
    }

    public static AccessRule Create(StorageAccount storageAccount, ResourceOwnerType resourceOwnerType, Guid resourceOwnerId, Bucket bucket, string rootPath = "", bool enabled = true, bool read = true, bool write = false, bool execute = false, string? description = "")
    {
        return new AccessRule(Guid.NewGuid(), storageAccount, resourceOwnerType, resourceOwnerId, bucket, rootPath, enabled, read, write, execute, description);
    }

    public AccessRule Update(string rootPath = "", bool enabled = true, bool read = true, bool write = false, bool execute = false, string? description = "")
    {
        bool isUpdated = false;


        if (IsEnabled != enabled)
        {
            IsEnabled = enabled;
            isUpdated = true;
        }

        if (Read != read)
        {
            Read = read;
            isUpdated = true;
        }

        if (Write != write)
        {
            Write = write;
            isUpdated = true;
        }

        if (Execute != execute)
        {
            Execute = execute;
            isUpdated = true;
        }

 
        if (!string.Equals(RootPath, rootPath, StringComparison.OrdinalIgnoreCase))
        {
            RootPath = RootPath ??= string.Empty;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description ??= string.Empty;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new AccessRuleUpdated { AccessRule = this });
        }

        return this;
    }

}
