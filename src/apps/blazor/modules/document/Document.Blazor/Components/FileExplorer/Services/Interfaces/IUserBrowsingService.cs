using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services.Interfaces;

public interface IUserBrowsingService
{
    /// <summary>
    /// Use User serice and AccressRules to get the user bucket and Folerd from Buckets including the StorageAccounts
    /// </summary>
    /// <returns></returns>
    System.Threading.Tasks.Task<System.Collections.Generic.ICollection<FolderModel>> GetRootFoldersAsync();
    /// <summary>
    /// Validae If the user Allowed To access this folder
    /// </summary>
    /// <param name="folder"></param>
    /// <returns></returns>
    System.Threading.Tasks.Task<bool> ValidateUserAccessAsync(FolderModel folder);
}
