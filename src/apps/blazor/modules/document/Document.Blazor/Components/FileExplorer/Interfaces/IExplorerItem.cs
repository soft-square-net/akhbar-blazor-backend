
using System.Net.Mime;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
using Microsoft.AspNetCore.Components;
using Shared.Enums;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
public interface IExplorerItem<T> where T : IExplorerItemModel
{
    [Parameter, EditorRequired] public T Model { get; set; }
}
