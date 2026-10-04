using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Shared.Authorization;
using static FSH.Starter.Shared.Authorization.TenantConstants;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Auth;
internal class ModulePermissions
{
    private static readonly FshPermission[] Permissions =
    [
        //documents
        new("View MudBlazorBlocks", ModuleActions.View, ModuleResources.MudBlazorBlocks, IsBasic: true, IsRoot:true),
        new("Search MudBlazorBlocks", ModuleActions.Search, ModuleResources.MudBlazorBlocks, IsBasic: true, IsRoot:true),
        new("Create MudBlazorBlocks", ModuleActions.Create, ModuleResources.MudBlazorBlocks),
        new("Update MudBlazorBlocks", ModuleActions.Update, ModuleResources.MudBlazorBlocks),
        new("Delete MudBlazorBlocks", ModuleActions.Delete, ModuleResources.MudBlazorBlocks),
        new("Export MudBlazorBlocks", ModuleActions.Export, ModuleResources.MudBlazorBlocks),

    ];
    public static IReadOnlyList<FshPermission> All { get; } = new ReadOnlyCollection<FshPermission>(Permissions);

}
