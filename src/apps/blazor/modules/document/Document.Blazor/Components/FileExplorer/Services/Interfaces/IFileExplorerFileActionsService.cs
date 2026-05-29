using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services.Interfaces;
public interface IFileExplorerFileActionsService
{
    public Guid Create(string name, byte[] data, string? location = "/",Dictionary<string,string>? meta = default);
    public Guid Copy(Guid Id, string destination);
    public FileModel GetById(Guid Id);
    public List<FileModel> SearchByName(string query);
    public List<FileModel> GetFolderFiles(string folder);
    public void DeleteFile(Guid Id);

}
