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
public interface ITreeNodeItem<TNode> where TNode : class
{
    string Id { get; }
    string? ParentId { get; set; }
    TNode? Parent { get; set; }
    List<TNode> Children { get; set; }
    bool IsExpanded { get; set; }
    bool IsVisible { get; set; }
}
