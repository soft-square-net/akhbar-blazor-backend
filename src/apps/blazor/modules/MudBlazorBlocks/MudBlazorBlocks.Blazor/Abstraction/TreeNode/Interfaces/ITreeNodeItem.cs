using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.TreeNode.Interfaces;
/// <summary>
/// Abstraction contract for hierarchical tree items used by MudTreeView.
/// </summary>
/// <typeparam name="TNode">The type of the node itself.</typeparam>
public interface ITreeNodeItem<TItem> where TItem : class, ITreeNodeItem<TItem>
{
    string Id { get; }
    string? ParentId { get; set; }
    TItem? Parent { get; set; }
    List<TItem> Children { get; set; }
    bool IsExpanded { get; set; }
    bool IsVisible { get; set; }

    string ToJson();
    static abstract TItem? FromJson(string json);
    static abstract string ToJsonList(IEnumerable<TItem> items);
    static abstract List<TItem> FromJsonList(string json);
}
