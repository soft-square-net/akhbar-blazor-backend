using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Base;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Notifications;

public interface IDocumentsStorageService
{
    Task<List<GetUserAccessRulesResponse>?> GetAccessRules();
    Task<FileStream> DownloadFile(FileModel model, string filePath, CancellationToken cancellationToken);
    Task<FileModel> UploadFile(Stream stream, string fileName, FolderModel folder, CancellationToken cancellationToken);
    Task<bool> Copy(List<BaseExplorerFactory> sources);
    Task<bool> Paste(FolderModel destination);
    Task<FolderModel> CreateFolder(string name);
    Task<FolderModel?> GetFolder(Bucket bucket,string path);
    /// <summary>
    /// It will be used in the path component to autocomplete the path when the user is typing it. It will return a list of folders where the path startsWith or equal this Path.
    /// Searches for folders in the specified path in all Buckets and returns a FolderModel representing the results folders where it's path startsWith or equal this Path.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    Task<FolderModel> SearchFolders(string path);

}
