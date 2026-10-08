using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.TreeNode.Interfaces;
/// <summary>
/// Service contract for managing MudTreeView state, drag-and-drop, and filtering.
/// </summary>
public interface IMudTreeService<TNode, TData>
    where TNode : class, ITreeNodeItem<TNode>
    where TData : class, new()
{
    List<TNode> RootNodes { get; }
    HashSet<string> ExpandedNodeIds { get; }

    event Action? OnTreeChanged;

    void SetTree(IEnumerable<TNode> items);

    // Drag and Drop Operations
    bool CanMoveNode(TNode sourceNode, TNode targetNode);
    bool MoveNode(TNode sourceNode, TNode? targetNode, int targetIndex = -1);

    // Filter Integration
    void ApplyFilter(Expression<Func<TreeNode<TData>, bool>> filterPredicate);
    void ApplyJsonFilter(string jsonFilter, bool combineWithAnd = true);
    void ClearFilter();

    // Node Utilities
    TNode? FindNodeById(string id);
    IEnumerable<TNode> GetFlattenedNodes();
    void ExpandAll();
    void CollapseAll();
}
