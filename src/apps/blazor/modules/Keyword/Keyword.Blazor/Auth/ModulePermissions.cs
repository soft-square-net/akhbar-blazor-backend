using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Shared.Authorization;
using static FSH.Starter.Shared.Authorization.TenantConstants;

namespace FSH.Starter.Blazor.Modules.Keyword.Blazor.Auth;
internal class ModulePermissions
{
    private static readonly FshPermission[] Permissions =
    [
        //documents
        new("View Keyword", ModuleActions.View, ModuleResources.Keyword, IsBasic: true, IsRoot:true),
        new("Search Keyword", ModuleActions.Search, ModuleResources.Keyword, IsBasic: true, IsRoot:true),
        new("Create Keyword", ModuleActions.Create, ModuleResources.Keyword),
        new("Update Keyword", ModuleActions.Update, ModuleResources.Keyword),
        new("Delete Keyword", ModuleActions.Delete, ModuleResources.Keyword),
        new("Export Keyword", ModuleActions.Export, ModuleResources.Keyword),

    ];
    public static IReadOnlyList<FshPermission> All { get; } = new ReadOnlyCollection<FshPermission>(Permissions);

}
