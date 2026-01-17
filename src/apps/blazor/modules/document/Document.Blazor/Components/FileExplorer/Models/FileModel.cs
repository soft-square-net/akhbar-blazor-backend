
using System.Globalization;
using System.Net.Mime;
using System.Reflection;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using Shared.Enums;
using static MudBlazor.Colors;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
public class FileModel : BaseExplorerItemModel
{
    public FileModel(Guid id, string name, long size, DateTime created, DateTime modified)
    {
        Id = id;
        Name = name;
        Size = size;
        Created = created;
        Modified = modified;
    }

    public Uri? Url { get; set; }
    public string? MimeType { get; set; }
    

    

    
    //public string Extension
    //{
    //    get { return Name.Split('.').Last(); }
    //}
    public string Extension { get { return System.IO.Path.GetExtension(Name).ToLowerInvariant(); } }

}
