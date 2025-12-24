
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Menues;
public partial class EditMenu
{
    private MudMenu _contextMenu = null!;

    private async Task OpenContextMenu(MouseEventArgs args)
    {
        await _contextMenu.OpenMenuAsync(args);
    }
}
