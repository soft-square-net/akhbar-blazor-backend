using System.Collections;
using Blazored.LocalStorage;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Infrastructure.Notifications;
using FSH.Starter.Blazor.Infrastructure.Notifications.Users;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.Explorers.Base;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using FSH.Starter.Shared.Authorization;
using Mapster;
using MediatR.Courier;
using OneOf.Types;
using static FSH.Starter.Blazor.Modules.Document.Blazor.FileExplorerIcons;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Notifications;

public class DocumentsStorageService: IDocumentsStorageService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly ICourier _courier;
    private readonly IApiClient _apiClient;

    public DocumentsStorageService(ILocalStorageService localStorageService, ICourier courier, IApiClient apiClient)
    {
        _localStorageService = localStorageService;
        _courier = courier;
        _apiClient = apiClient;
        _courier.SubscribeWeak<NotificationWrapper<UserLoggedIn>>(HandleUserLoggedIn);
        _courier.SubscribeWeak<NotificationWrapper<UserLoggedOut>>(HandleUserLoggedOut);

    }

    private async Task HandleUserLoggedIn(NotificationWrapper<UserLoggedIn> wrapper)
    {
        var user = wrapper.Notification.UserInfo;
        if (user == null) return;
        var userGuid = new Guid(user.GetUserId() ?? Guid.Empty.ToString());
        var accessRules = await _apiClient.GetUserAccessRulesEndpointAsync(userGuid);
        // Do something with the user ID, e.g., store it in local storage
        await _localStorageService.SetItemAsync("accessRules", accessRules);
    }


    private async Task HandleUserLoggedOut(NotificationWrapper<UserLoggedOut> wrapper)
    {
        await _localStorageService.RemoveItemAsync("accessRules");
    }

    public async Task<List<GetUserAccessRulesResponse>?> GetAccessRules()
    {
        return await _localStorageService.GetItemAsync<List<GetUserAccessRulesResponse>>("accessRules");
    }

    private async Task<bool> CanReadRule(BaseExplorerItemModel model) {  return false; }
    private async Task<bool> CanWriteRule(FolderModel model) {  return false; }
    private async Task<bool> CanDeleteRule(BaseExplorerItemModel model) {  return false; }
    private async Task<bool> CanExecuteRule() {  return false; }

    public async Task<FileStream> DownloadFile(FileModel model, string filePath, CancellationToken cancellationToken)
    {
        //GetBucketFileResponse? fileData = await _apiClient.GetBucketFileEndpointAsync(model.Folder.BucketId, model.Folder.Id, model.Id, cancellationToken);
        DownloadBucketFileResponse fileData = await _apiClient.DownloadBucketFileEndpointAsync(model.Folder.BucketId, model.Id);

        // Create the FileStream and write the bytes to disk
        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            if (fileData?.Stream != null)
            {
                await fs.WriteAsync(fileData.Stream.AsMemory(0, fileData.Stream.Length), cancellationToken);
            }
        }
        return new FileStream(filePath, FileMode.Open, FileAccess.Read);
    }

    public async Task<FileModel> UploadFile(FileParameter fileParameter, string fileName, FolderModel folder, int fileSize, CancellationToken cancellationToken)
    {
        var fileModel = new FileModel(Guid.NewGuid(), fileName, fileSize, DateTime.Now, DateTime.Now);
        FileType fileType = (FileType)Enum.Parse(typeof(FileType), Enum.GetName(typeof(global::Shared.Enums.FileType), fileModel.GetFileType())!, true);
        // var file = await _apiClient.CreateBucketFileEndpointAsync(folder.BucketId, folder.Id, fileType, new FileParameter(stream, fileName, fileModel.GetMIMEType()));
        var file = await _apiClient.CreateBucketFileEndpointAsync(folder.BucketId, folder.Id, fileType, fileParameter);
        // GetBucketFileResponse? result = await _apiClient.GetBucketFileEndpointAsync(folder.BucketId, folder.Id, (Guid)file.Id);
        // GetBucketFileResponse? result = await _apiClient.GetBucketFileEndpointAsync(folder.BucketId, folder.Id, Guid.NewGuid());
        try
        {
            var result = await _apiClient.GetBucketFileEndpointAsync(folder.BucketId, folder.Id, file.FileId, cancellationToken);
            return result.Adapt<FileModel>();
        }
        catch (FSH.Starter.Blazor.Infrastructure.Api.ApiException ex)
        {
            Console.Error.WriteLine("Status: " + ex.StatusCode);
            Console.Error.WriteLine("Response body raw: " + (ex.Response ?? "<null or empty>"));

            // Guard example: treat empty response as missing content
            if (string.IsNullOrWhiteSpace(ex.Response))
            {
                // handle appropriately: return null, throw more specific, or try alternative endpoint
                throw new InvalidOperationException("GetBucketFile returned an empty body.");
            }

            throw;
        }


    }
    public Task<bool> Copy(List<BaseExplorerFactory> sources)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Paste(FolderModel destination)
    {
        throw new NotImplementedException();
    }

    public Task<FolderModel> CreateFolder(string name)
    {
        throw new NotImplementedException();
    }

    public Task<FolderModel?> GetFolder(Bucket bucket, string path)
    {
        throw new NotImplementedException();
    }

    public Task<FolderModel> SearchFolders(string path)
    {
        throw new NotImplementedException();
    }
}
