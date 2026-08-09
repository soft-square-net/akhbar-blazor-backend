using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.Infrastructure.Auth;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services.Interfaces;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services;

public class UserBrowsingService : IUserBrowsingService
{
    private readonly ICurrentUserService _currentUserService;

    public UserBrowsingService(ICurrentUserService userSrv)
    {
        _currentUserService = userSrv;
    }

    public async Task<ICollection<FolderModel>> GetRootFoldersAsync()
    {
        var user = await _currentUserService.GetUserInfoAsync();
        // TODO: use user to filter buckets/folders based on access rules
        // For now return an empty list or default root set
        return new List<FolderModel>();
    }

    public async Task<bool> ValidateUserAccessAsync(FolderModel folder)
    {
        var user = await _currentUserService.GetUserInfoAsync();
        // TODO: implement actual access validation using AccessRules and user info
        return user is not null;
    }
}
