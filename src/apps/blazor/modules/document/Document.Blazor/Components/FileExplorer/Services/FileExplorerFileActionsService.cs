using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services.Interfaces;
using FSH.Starter.Blazor.Infrastructure.Api;
using Microsoft.Extensions.Configuration;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services;

public class FileExplorerFileActionsService : IFileExplorerFileActionsService, IDisposable
{
    private IApiClient? _client;

    public FileExplorerFileActionsService(HttpClient http)
    {
        _client = new ApiClient(http);
    }
    public Guid Copy(Guid Id, string destination)
    {
        throw new NotImplementedException();
    }

    public Guid Create(string name, byte[] data, string? location = "/", Dictionary<string, string>? meta = null)
    {
        throw new NotImplementedException();
    }

    public void DeleteFile(Guid Id)
    {
        throw new NotImplementedException();
    }

    

    public FileModel GetById(Guid Id)
    {
        throw new NotImplementedException();
    }

    public List<FileModel> GetFolderFiles(string folder)
    {
        throw new NotImplementedException();
    }

    public List<FileModel> SearchByName(string query)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        _client = null; 
    }
}
