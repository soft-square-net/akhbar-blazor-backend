
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using FSH.Starter.Blazor.Infrastructure.Auth;
using MudBlazorLayout.Auth;
using BlazorModularEngine.Core.Common.Interfaces;
using BlazorModularEngine.Core.Services.Interfaces;

namespace MudBlazorLayout.Components.Menu;
public partial class NavMenu: IModuleMenu
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;

    [Inject]
    protected IModulesLoader ModuleLoaderService { get; set; } = default!;

    public string Name => "Document";

    public string Title => "Documents";

    public string Description => "Manage Documents, Files, Folders and Buckets";

    public int Order => 100;

    private bool _canViewBuckets;
    private bool _canViewDocuments;
    private bool _canViewFiles;
    private bool _canViewFolders;
    private bool _canViewStorageAccounts;
    private bool CanViewBucketsGroup => _canViewFiles || _canViewFolders || _canViewStorageAccounts;



    protected override async Task OnParametersSetAsync()
    {
        var user = (await AuthState).User;
        _canViewBuckets = await AuthService.HasPermissionAsync(user, ModuleActions.View, ModuleResources.Buckets);
        _canViewDocuments = await AuthService.HasPermissionAsync(user, ModuleActions.View, ModuleResources.Documents);
        _canViewFiles = await AuthService.HasPermissionAsync(user, ModuleActions.View, ModuleResources.Files);
        _canViewFolders = await AuthService.HasPermissionAsync(user, ModuleActions.View, ModuleResources.Folders);
        _canViewStorageAccounts = await AuthService.HasPermissionAsync(user, ModuleActions.View, ModuleResources.StorageAccounts);
    }
}
