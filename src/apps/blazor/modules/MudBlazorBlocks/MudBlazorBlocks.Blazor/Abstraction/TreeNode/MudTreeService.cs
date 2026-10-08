using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.TreeNode.Interfaces;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.TreeNode;
public class MudTreeService<TNode, TData> : IMudTreeService<TNode, TData>
    where TNode : class, ITreeNodeItem<TNode>
    where TData : class, new ()
{
    public List<TNode> RootNodes { get; private set; } = new();
public HashSet<string> ExpandedNodeIds { get; private set; } = new();

public event Action? OnTreeChanged;

public void SetTree(IEnumerable<TNode> items)
{
    RootNodes = items.ToList();
    NotifyStateChanged();
}

#region Drag and Drop Logic

public bool CanMoveNode(TNode sourceNode, TNode targetNode)
{
    ArgumentNullException.ThrowIfNull(sourceNode);
    ArgumentNullException.ThrowIfNull(targetNode);

    if (sourceNode.Id == targetNode.Id) return false;

    // Prevent moving a parent into its own sub-tree descendant
    var current = targetNode.Parent;
    while (current != null)
    {
        if (current.Id == sourceNode.Id) return false;
        current = current.Parent;
    }

    return true;
}

public bool MoveNode(TNode sourceNode, TNode? targetNode, int targetIndex = -1)
{
    ArgumentNullException.ThrowIfNull(sourceNode);

    if (targetNode != null && !CanMoveNode(sourceNode, targetNode))
    {
        return false;
    }

    // 1. Remove source node from its current parent/root collection
    if (sourceNode.Parent != null)
    {
        sourceNode.Parent.Children.Remove(sourceNode);
    }
    else
    {
        RootNodes.Remove(sourceNode);
    }

    // 2. Attach to new target (or root if target is null)
    if (targetNode != null)
    {
        sourceNode.Parent = targetNode;
        sourceNode.ParentId = targetNode.Id;

        if (targetIndex >= 0 && targetIndex <= targetNode.Children.Count)
            targetNode.Children.Insert(targetIndex, sourceNode);
        else
            targetNode.Children.Add(sourceNode);
    }
    else
    {
        sourceNode.Parent = null;
        sourceNode.ParentId = null;

        if (targetIndex >= 0 && targetIndex <= RootNodes.Count)
            RootNodes.Insert(targetIndex, sourceNode);
        else
            RootNodes.Add(sourceNode);
    }

    NotifyStateChanged();
    return true;
}

#endregion

#region Filtering Integration

public void ApplyFilter(Expression<Func<TreeNode<TData>, bool>> filterPredicate)
{
    var compiledFilter = filterPredicate.Compile();

    foreach (var root in RootNodes)
    {
        EvaluateVisibilityRecursively(root, compiledFilter);
    }

    NotifyStateChanged();
}

public void ApplyJsonFilter(string jsonFilter, bool combineWithAnd = true)
{
    var filterExpr = TreeNodeFilterBuilder<TData>.ParseJsonFilter(jsonFilter, combineWithAnd);
    ApplyFilter(filterExpr);
}

public void ClearFilter()
{
    foreach (var node in GetFlattenedNodes())
    {
        node.IsVisible = true;
    }

    NotifyStateChanged();
}

private bool EvaluateVisibilityRecursively(TNode node, Func<TreeNode<TData>, bool> filter)
{
    // Wrap node state into a temporary TreeNode<TData> for predicate validation
    var tempTreeNode = new TreeNode<TData>();
    bool matchesSelf = filter(tempTreeNode);

    bool hasVisibleChild = false;
    foreach (var child in node.Children)
    {
        if (EvaluateVisibilityRecursively(child, filter))
        {
            hasVisibleChild = true;
        }
    }

    node.IsVisible = matchesSelf || hasVisibleChild;

    if (hasVisibleChild)
    {
        node.IsExpanded = true;
        ExpandedNodeIds.Add(node.Id);
    }

    return node.IsVisible;
}

#endregion

#region Tree Utilities

public TNode? FindNodeById(string id)
{
    return GetFlattenedNodes().FirstOrDefault(n => n.Id == id);
}

public IEnumerable<TNode> GetFlattenedNodes()
{
    var queue = new Queue<TNode>(RootNodes);
    while (queue.Count > 0)
    {
        var current = queue.Dequeue();
        yield return current;

        foreach (var child in current.Children)
        {
            queue.Enqueue(child);
        }
    }
}

public void ExpandAll()
{
    foreach (var node in GetFlattenedNodes())
    {
        node.IsExpanded = true;
        ExpandedNodeIds.Add(node.Id);
    }
    NotifyStateChanged();
}

public void CollapseAll()
{
    foreach (var node in GetFlattenedNodes())
    {
        node.IsExpanded = false;
    }
    ExpandedNodeIds.Clear();
    NotifyStateChanged();
}

private void NotifyStateChanged() => OnTreeChanged?.Invoke();

    #endregion
}
