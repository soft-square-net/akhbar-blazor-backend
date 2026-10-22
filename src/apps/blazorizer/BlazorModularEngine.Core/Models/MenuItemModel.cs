using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorModularEngine.Core.Models;

public class MenuItemModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string? Href { get; set; }                  // Nullable since folder headers don't need direct links
    public string Icon { get; set; } = string.Empty;    // Framework-agnostic or MudBlazor icon constant
    public string Region { get; set; } = "sidebar";     // "sidebar", "header", "footer"
    public int Order { get; set; } = 100;

    // Recursive nested items
    public List<MenuItemModel> Children { get; set; } = new();

    public bool HasChildren => Children != null && Children.Count > 0;
}
