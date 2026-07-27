using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Components.FileBrowser.Base;

public class BaseBrowserCard<T> : ComponentBase, IExplorerItem<T> where T : BaseExplorerItemModel
{
    protected bool IsLoading { get; set; }
    [Parameter, EditorRequired] public T Model { get; set; }
}
