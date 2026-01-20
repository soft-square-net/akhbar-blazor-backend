
using System.Collections.ObjectModel;
using FSH.Starter.Shared.Authorization;

namespace FSH.Starter.Blazor.Modules.DesktopLayout.Blazor.Auth;
internal class ModulePermissions
{
    private static readonly FshPermission[] Permissions =
    [
        //documents
        new("View Documents", ModuleActions.View, ModuleResources.Documents, IsBasic: true),
        new("Search Documents", ModuleActions.Search, ModuleResources.Documents, IsBasic: true),
        new("Create Documents", ModuleActions.Create, ModuleResources.Documents),
        new("Update Documents", ModuleActions.Update, ModuleResources.Documents),
        new("Delete Documents", ModuleActions.Delete, ModuleResources.Documents),
        new("Export Documents", ModuleActions.Export, ModuleResources.Documents),

        //StorageAccounts
        new("View StorageAccounts", ModuleActions.View, ModuleResources.StorageAccounts, IsBasic: true),
        new("Search StorageAccounts", ModuleActions.Search, ModuleResources.StorageAccounts, IsBasic: true),
        new("Create StorageAccounts", ModuleActions.Create, ModuleResources.StorageAccounts),
        new("Update StorageAccounts", ModuleActions.Update, ModuleResources.StorageAccounts),
        new("Delete StorageAccounts", ModuleActions.Delete, ModuleResources.StorageAccounts),
        new("Export StorageAccounts", ModuleActions.Export, ModuleResources.StorageAccounts),

        //Buckets
        new("View Buckets", ModuleActions.View, ModuleResources.Buckets, IsBasic: true),
        new("Search Buckets", ModuleActions.Search, ModuleResources.Buckets, IsBasic: true),
        new("Create Buckets", ModuleActions.Create, ModuleResources.Buckets),
        new("Update Buckets", ModuleActions.Update, ModuleResources.Buckets),
        new("Delete Buckets", ModuleActions.Delete, ModuleResources.Buckets),
        new("Export Buckets", ModuleActions.Export, ModuleResources.Buckets),

        //Files
        new("View Files", ModuleActions.View, ModuleResources.Files, IsBasic: true),
        new("Search Files", ModuleActions.Search, ModuleResources.Files, IsBasic:true),
        new("Create Files", ModuleActions.Create, ModuleResources.Files),
        new("Update Files", ModuleActions.Update, ModuleResources.Files),
        new("Delete Files", ModuleActions.Delete, ModuleResources.Files),
        new("Export Files", ModuleActions.Export, ModuleResources.Files),

        //Folders
        new("View Folders", ModuleActions.View, ModuleResources.Folders, IsBasic: true),
        new("Search Folders", ModuleActions.Search, ModuleResources.Folders, IsBasic: true),
        new("Create Folders", ModuleActions.Create, ModuleResources.Folders),
        new("Update Folders", ModuleActions.Update, ModuleResources.Folders),
        new("Delete Folders", ModuleActions.Delete, ModuleResources.Folders),
        new("Export Folders", ModuleActions.Export, ModuleResources.Folders),

    ];
    public static IReadOnlyList<FshPermission> All { get; } = new ReadOnlyCollection<FshPermission>(Permissions);

}
