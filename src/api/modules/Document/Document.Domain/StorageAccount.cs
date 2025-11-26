
using System.Xml.Linq;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Document.Domain.Events;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Domain;
public class StorageAccount: AuditableEntity, IAggregateRoot
{
    public StorageProvider Provider { get; private set; }
    public string AccountName { get; private set; }
    public string AccessKey { get; private set; } = string.Empty;
    public string SecretKey { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public ICollection<Bucket> Buckets { get; private set; }



    private StorageAccount() { }

    private StorageAccount(Guid id, StorageProvider storageProvider, string accountName, string? accessKey, string? secretKey, string? description)
    {
        Id = id;
        Provider = storageProvider;
        AccountName = accountName;
        AccessKey = accessKey ??= string.Empty;
        SecretKey = secretKey ??= string.Empty;
        Description = description ??= string.Empty;
        QueueDomainEvent(new StorageAccountCreated { StorageAccount = this });
    }

    public static StorageAccount Create(StorageProvider storageProvider, string accountName, string? accessKey, string? secretKey, string? description)
    {
        return new StorageAccount(Guid.NewGuid(), storageProvider, accountName, accessKey, secretKey, description);
    }

    public StorageAccount Update(string? accountName, string? accessKey, string? secretKey, string? description)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(accountName) && !string.Equals(AccountName, accountName, StringComparison.OrdinalIgnoreCase))
        {
            AccountName = accountName;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(accessKey) && !string.Equals(AccessKey, accessKey, StringComparison.OrdinalIgnoreCase))
        {
            AccessKey = accessKey;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(secretKey) && !string.Equals(SecretKey, secretKey, StringComparison.OrdinalIgnoreCase))
        {
            SecretKey = secretKey;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description ??= string.Empty;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new StorageAccountUpdated { StorageAccount = this });
        }

        return this;
    }

}
