using System.ComponentModel;
using System.Xml.Linq;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Document.Domain.Events;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Domain;
public class Bucket : AuditableEntity, IAggregateRoot
{
    public StorageAccount StorageAccount { get; private set; }
    public string Region { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    // public Folder RootFolder { get; private set; }

    [Description("(ARN) in AmazoneS3 Bucket, FullPath of Parent Folder in Local File System )")]
    public string? ResourceName { get; private set; }
    // Update Size via Domain Event from File entity
    public long Size { get; private set; }
    // Update MaxSize via Update Method or Settings or Configurations
    public long MaxSize { get; private set; }
    public string? Description { get; private set; }
    public ICollection<Folder> Folders { get; private set; } = new List<Folder>();  

    private Bucket() { }

    private Bucket(Guid id, StorageAccount storageAccount, string region, string key, string name, string resorceName, string? description, long size=0, long maxSize=0)
    {
        Id = id;
        StorageAccount = storageAccount;
        Region = region;
        Name = name;
        ResourceName = resorceName;
        Size = size;
        MaxSize = maxSize;
        Description = description;
        // initialize the root folder
        // RootFolder = Folder.Create(this, null, "root",);
        QueueDomainEvent(new BucketCreated { Bucket = this });
    }

    public static Bucket Create(StorageAccount storageAccount, string region, string key, string name, string resorceName, string? description, long size = 0, long maxSize = 0)
    {
        var bkt = new Bucket(Guid.NewGuid(), storageAccount, region, key, name,resorceName, description, size, maxSize);
        bkt.Folders.Add(Folder.Create(bkt, ""));
        return bkt;
    }

    public Bucket UpdateSize(long size, long maxSize)
    {
        bool isUpdated = false;

        if (size > 0 && Size != size)
        {
            Size = size;
            isUpdated = true;
        }
        if (maxSize > 0 && MaxSize != maxSize)
        {
            MaxSize = maxSize;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new BucketUpdated { Bucket = this });
        }
        return this;
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

    public Bucket AddFolder(Folder folder)
    {
        Folders.Add(folder);
        return this;
    }
    public Bucket UpdateFolder(Folder folder) { 
        Folder? folderToUpdate =Folders.SingleOrDefault(f => f.Id == folder.Id);
        if (folderToUpdate != null) { 
            folderToUpdate.Update(folder.Name, folder.Icon, folder.Description);
        }
        return this;
    }
    public Bucket AddFile(Folder folder,File file)
    {
        Folder? folderToUpdate = Folders.SingleOrDefault(f => f.Id == folder.Id);
        if (folderToUpdate != null)
        {
            folderToUpdate.AddFile(file);
        }
        return this;
    }
    public Bucket UpdateFile(File file)
    {
        if (file.Folder == null) return this;
        Folder? folderToUpdate = Folders.SingleOrDefault(f => f.Id == file.Folder.Id);
        if (folderToUpdate != null && folderToUpdate.Id == file.Folder.Id)
        {
            folderToUpdate.UpdateFile(file);
        }
        return this;
    }

}
