using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudBlazor;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;

public class TreeItemDataModel
{
    public string Name { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;
    public bool IsFolder { get; set; }
    public IReadOnlyCollection<TreeItemData<TreeItemDataModel>> Children { get; set; } = new List<TreeItemData<TreeItemDataModel>>();
}
