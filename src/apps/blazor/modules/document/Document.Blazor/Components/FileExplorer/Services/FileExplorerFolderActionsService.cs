using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services.Interfaces;
using FSH.Starter.Blazor.Infrastructure.Api;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services;

public class FileExplorerFolderActionsService : IFileExplorerFolderActionsService
{
    private IApiClient? _client;

    public FileExplorerFolderActionsService(HttpClient http)
    {
        _client = new ApiClient(http);
    }
    public FolderModel Copy(Guid Id, string destination)
    {
        throw new NotImplementedException();
    }

    public Guid Create(string name, string path)
    {
        throw new NotImplementedException();
    }

    public void DeleteFolder(Guid Id)
    {
        throw new NotImplementedException();
    }

    public List<FolderModel> GetById(Guid Id)
    {
        throw new NotImplementedException();
    }

    public List<FolderModel> SearchByName(string query)
    {
        throw new NotImplementedException();
    }

    public List<FolderModel> UploadFile(string query)
    {
        throw new NotImplementedException();
    }
}
