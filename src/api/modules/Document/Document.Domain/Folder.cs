using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Document.Domain.Events;

namespace FSH.Starter.WebApi.Document.Domain;
public class Folder : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string Icon { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? FullPath { get; private set; }
    public Bucket Bucket { get; private set; }
    public Folder? Parent { get; private set; }
    public bool IsRoot { get; private set; }
    public ICollection<Folder?>? Children { get; private set; }
    public ICollection<File?>? Files { get; private set; }

    private Folder() { }

    private Folder(Guid id, Bucket bucket, string name, string? icon, string? description, Folder? parent = null)
    {
        Id = id;
        Bucket = bucket;
        Name = name;
        Icon = icon ??= string.Empty;
        Description = description;
        Parent = parent;
        IsRoot = parent == null;
        QueueDomainEvent(new FolderCreated { Folder = this });
    }

    internal static Folder Create(Bucket bucket, string icon="")
    {
        return new Folder(
            Guid.NewGuid(), 
            bucket, 
            "/", 
            icon, 
            $"Root folder for the {bucket.Name}"
        );
    }

    public Folder AddChildFolder(string name, string? icon, string? description)
    {
        if (Children == null)
        {
            Children = new List<Folder?>();
        }
        Children.Add( new Folder(Guid.NewGuid(),this.Bucket, name, icon, description, this));
        return this;
    }
    public Folder UpdateChildFolder(Folder child)
    {
        if (Children == null)
        {
            return this;
        }
        Folder? folderToUpdate = Children.SingleOrDefault(f => f.Id == child.Id);
        if (folderToUpdate != null)
        {
            folderToUpdate.Update(child.Name,child.Icon,child.Description);
        }
        return this;
    }
    public Folder AddFile(File file)
    {
        if (Files == null)
        {
            Files = new List<File?>();
        }
        Files.Add(file);
        return this;
    }

    public Folder UpdateFile(File file)
    {
        if (Files == null )
        {
            return this;
        }
        File? fileToUpdate = Files.SingleOrDefault(f => f!.Id == file.Id);        if (fileToUpdate != null)
        if(fileToUpdate != null)
            {
            fileToUpdate.Update(file.Name, file.Description);
        }
        return this;
    }

    // /* only neded if you want to remove files from folder or bucket physically;*/
    // public Folder RemoveFile(Guid Id)
    // {
    //     if (Files == null)
    //     {
    //         return this;
    //     }
    //     var fileToRemove = Files.FirstOrDefault(f => f.Id == Id);
    //     if (fileToRemove != null)
    //     {
    //         Files.Remove(fileToRemove);
    //     }
    //     return this;
    // }

    public Folder Update(string? name, string? icon, string? description)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(icon) && !string.Equals(Icon, icon, StringComparison.OrdinalIgnoreCase))
        {
            Icon = icon;
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

    public string GetFullPath()
    {
        if (Parent == null)
        {
            return Name;
        }
        return $"{Parent.GetFullPath()}/{Name}";
    }

    public Folder SetFullPath(string fullPath="")
    {
        bool isUpdated = false;
        if (string.IsNullOrWhiteSpace(fullPath)) {
            fullPath = GetFullPath().Trim();
            isUpdated = true;
        }
        else
        {
            if (!string.Equals(FullPath, fullPath, StringComparison.OrdinalIgnoreCase))
            {
                FullPath = fullPath;
                isUpdated = true;
            }
        }

        if (isUpdated)
        {
            QueueDomainEvent(new FolderPathChanged { Folder = this });
        }
        return this;
    }
}
