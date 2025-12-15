using System.ComponentModel;
using System.Net.Sockets;
using System.Xml.Linq;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Framework.Core.Storage.File;
using FSH.Starter.WebApi.Document.Domain.Events;
using MediatR;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Domain;
public class Bucket : AuditableEntity, IAggregateRoot
{
    private readonly List<Domain.Folder> _Folders = new();
    public Guid StorageAccountId { get; private set; }
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


    public StorageAccount StorageAccount { get; private set; } = null!;
    public IReadOnlyList<Domain.Folder> Folders  =>  _Folders.ToList();  

    private Bucket() { }

    private Bucket(Guid id, StorageAccount storageAccount, string region, string name, string resorceName, string? description, long size=0, long maxSize=0)
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
        this._Folders.Add(Folder.Create(this));
    }
    public static Bucket Create(StorageAccount storageAccount, string region, string name, string resorceName, string? description, long size = 0, long maxSize = 0)
    {
        var bkt = new Bucket(Guid.NewGuid(), storageAccount, region, name,resorceName, description, size, maxSize);
       
        return bkt;
    }

    private void UpdateSize(long size, long maxSize)
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
        // return this;
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

    
    public Bucket UpdateFolder(Folder folder) { 
        Folder? folderToUpdate =Folders.SingleOrDefault(f => f.Id == folder.Id);
        if (folderToUpdate != null) { 
            folderToUpdate.Update(folder.Name, folder.Icon, folder.Description);
        }
        return this;
    }
    public void AddFile(Guid folderId, string key, string name, string extension, string fileUrl, FileType fileType, long size, bool isPublic, string? description = null) { 
        // File file = new File(Guid.NewGuid(), folderId, key, name, extension, fileUrl, fileType, size, isPublic, description);
        // File file = File.Create(Guid.NewGuid(), folderId, key, name, extension, fileUrl, fileType, size, isPublic, description);
        Folders.SingleOrDefault(f => f.Id == folderId)?
            .AddFile( key, name, extension, fileUrl, fileType, size, isPublic, description);
        UpdateSize(Size + size, MaxSize);
    }
    //public void AddFile(Folder folder,File file)
    //{
    //    Folders.SingleOrDefault(f => f.Id == folder.Id)?.Files?.Add(file);
    //    // return this;
    //}
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
