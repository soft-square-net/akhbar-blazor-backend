using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services.Interfaces;
public interface IFileExplorerFolderActionsService
{
    public Guid Create(string name, string path);
    public FolderModel Copy(Guid Id, string destination);
    public List<FolderModel> GetById(Guid Id);
    public List<FolderModel> SearchByName(string query);
    public List<FolderModel> UploadFile(string query);
    // public List<FolderModel> GetSubFolders(string folder);
    public void DeleteFolder(Guid Id);
}
