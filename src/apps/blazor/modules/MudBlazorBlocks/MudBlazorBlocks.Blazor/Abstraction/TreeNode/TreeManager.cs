
using global::FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.TreeNode.Interfaces;
using MudBlazor;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.TreeNode;
public class TreeManager<TNode> where TNode : class, ITreeNodeAdapter<TNode>
{
    public List<TNode> RootNodes { get; private set; } = new();
    public List<TNode> FlatNodes { get; private set; } = new();
    public List<ITreeItemData<TNode>> TreeItemData { get; private set; } = new();

    // Drag-and-Drop State
    public TNode? DraggedNode { get; private set; }
    public TNode? TargetNode { get; private set; }
    public bool IsOverRoot { get; private set; }

    public event Action? OnStateChanged;
    public TNode? SelectedNode { get; private set; }

    public void SelectNode(TNode node)
    {
        SelectedNode = node;
        OnStateChanged?.Invoke();
    }

    public void Initialize(List<TNode> rootNodes)
    {
        RootNodes = rootNodes;
        Refresh();
    }

    public void Refresh()
    {
        FlatNodes = Flatten(RootNodes).ToList();
        TreeItemData = BuildTreeItemData(RootNodes);
        OnStateChanged?.Invoke();
    }

    #region Drag & Drop Operations

    public void HandleDragStart(TNode node) => DraggedNode = node;

    public void HandleDragOver(TNode node)
    {
        if (DraggedNode == null || DraggedNode == node || IsDescendant(DraggedNode, node.Id))
            return;
        // Only update and notify when transitioning to a NEW target node
        if (TargetNode != node)
        {
            TargetNode = node;
            IsOverRoot = false;
            OnStateChanged?.Invoke();
        }
    }

    public void HandleDragLeave(TNode node)
    {
        // Only clear if leaving the current active target node
        if (TargetNode == node)
        {
            TargetNode = null;
            OnStateChanged?.Invoke();
        }
    }
    public void HandleDragOverRoot()
    {
        if (DraggedNode != null && (!IsOverRoot || TargetNode != null))
        {
            IsOverRoot = true;
            TargetNode = null;
            OnStateChanged?.Invoke();
        }
    }

    public void HandleDragLeaveRoot() => IsOverRoot = false;

    public void HandleDropOnNode(TNode targetNode)
    {
        if (DraggedNode == null || targetNode == null) return;
        if (DraggedNode == targetNode || IsDescendant(DraggedNode, targetNode.Id)) return;

        var droppedNode = DraggedNode; // Store reference to the moved node

        UnlinkNode(DraggedNode);

        DraggedNode.Parent = targetNode;
        DraggedNode.ParentId = targetNode.Id;

        if (!targetNode.Children.Contains(DraggedNode))
        {
            targetNode.Children.Add(DraggedNode);
            targetNode.AddChildData(DraggedNode);
        }

        targetNode.IsExpanded = true;

        // Automatically select the dropped item
        SelectedNode = droppedNode;

        ResetDragState();
        Refresh();
    }

    public void HandleDropOnRoot()
    {
        if (DraggedNode == null) return;

        var droppedNode = DraggedNode; // Store reference to the moved node

        UnlinkNode(DraggedNode);

        DraggedNode.Parent = null;
        DraggedNode.ParentId = null;

        if (!RootNodes.Contains(DraggedNode))
        {
            RootNodes.Add(DraggedNode);
        }

        // Automatically select the dropped item
        SelectedNode = droppedNode;

        ResetDragState();
        Refresh();
    }

    public void ResetDragState()
    {
        DraggedNode = null;
        TargetNode = null;
        IsOverRoot = false;
    }

    private void UnlinkNode(TNode node)
    {
        if (node.Parent != null)
        {
            node.Parent.Children.Remove(node);
            node.Parent.RemoveChildData(node);
        }
        else
        {
            RootNodes.Remove(node);
        }
    }

    public bool IsDescendant(TNode parent, string targetId)
    {
        foreach (var child in parent.Children)
        {
            if (child.Id == targetId || IsDescendant(child, targetId))
                return true;
        }
        return false;
    }

    #endregion

    #region Search & Expansion Actions

    public void ApplyFilter(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            foreach (var node in FlatNodes)
            {
                node.IsVisible = true;
            }
        }
        else
        {
            string trimmed = query.Trim();
            foreach (var root in RootNodes)
            {
                EvaluateVisibility(root, trimmed);
            }
        }
        Refresh();
    }

    private bool EvaluateVisibility(TNode node, string query)
    {
        bool matches = node.MatchesQuery(query);
        bool hasVisibleChild = false;

        foreach (var child in node.Children)
        {
            if (EvaluateVisibility(child, query))
            {
                hasVisibleChild = true;
            }
        }

        node.IsVisible = matches || hasVisibleChild;
        if (hasVisibleChild) node.IsExpanded = true;
        return node.IsVisible;
    }

    public void ExpandAll()
    {
        foreach (var node in FlatNodes) node.IsExpanded = true;
        Refresh();
    }

    public void CollapseAll()
    {
        foreach (var node in FlatNodes) node.IsExpanded = false;
        Refresh();
    }

    #endregion

    #region Data Conversion Helpers

    private List<ITreeItemData<TNode>> BuildTreeItemData(IEnumerable<TNode> nodes)
    {
        var list = new List<ITreeItemData<TNode>>();
        foreach (var node in nodes.Where(n => n.IsVisible))
        {
            list.Add(new TreeItemData<TNode>
            {
                Value = node,
                Expanded = node.IsExpanded,
                Children = BuildConcreteTreeItemData(node.Children)
            });
        }
        return list;
    }

    private List<TreeItemData<TNode>> BuildConcreteTreeItemData(IEnumerable<TNode> nodes)
    {
        var list = new List<TreeItemData<TNode>>();
        foreach (var node in nodes.Where(n => n.IsVisible))
        {
            list.Add(new TreeItemData<TNode>
            {
                Value = node,
                Expanded = node.IsExpanded,
                Children = BuildConcreteTreeItemData(node.Children)
            });
        }
        return list;
    }

    public IEnumerable<TNode> Flatten(IEnumerable<TNode> nodes)
    {
        foreach (var node in nodes)
        {
            yield return node;
            foreach (var child in Flatten(node.Children))
            {
                yield return child;
            }
        }
    }

    #endregion
}
