using Blazored.LocalStorage;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Infrastructure.Notifications;
using FSH.Starter.Blazor.Infrastructure.Notifications.Users;
using FSH.Starter.Shared.Authorization;
using MediatR.Courier;

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

    }

    private async Task HandleUserLoggedIn(NotificationWrapper<UserLoggedIn> wrapper)
    {
        var user = wrapper.Notification.UserInfo;
        if (user == null) return;
        var userGuid = new Guid(user.GetUserId() ?? Guid.Empty.ToString());
        var accessRules = await _apiClient.GetUserAccessRulesEndpointAsync(userGuid);
        // Do something with the user ID, e.g., store it in local storage
        await _localStorageService.SetItemAsync("accessRules", accessRules);
        var rules = await _localStorageService.GetItemAsync<List<GetUserAccessRulesResponse>>("accessRules");
    }
}
