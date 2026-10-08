
using System.Text.Json;
using global::FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.TreeNode.Interfaces;
using MudBlazor;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.TreeNode;
public class TreeManager<TNode> where TNode : class, ITreeNodeAdapter<TNode>
{
    public List<TNode> RootNodes { get; private set; } = new();
    public List<TNode> FlatNodes { get; private set; } = new();
    public List<ITreeItemData<TNode>> TreeItemData { get; private set; } = new();

    // Drag-and-Drop State
    
    public TNode? SelectedNode { get; private set; }
    public TNode? DraggedNode { get; private set; }
    public TNode? TargetNode { get; private set; }
    public bool IsOverRoot { get; private set; }

    public event Action? OnStateChanged;



    public void Initialize(List<TNode> rootNodes)
    {
        RootNodes = rootNodes;
        SelectedNode = null;
        Refresh();
    }

    public void Refresh()
    {
        RecalculateHierarchyMetadata();
        FlatNodes = Flatten(RootNodes).ToList();
        TreeItemData = BuildTreeItemData(RootNodes);
        OnStateChanged?.Invoke();
    }

    public void SelectNode(TNode node)
    {
        SelectedNode = node;
        OnStateChanged?.Invoke();
    }
 
    /// <summary>
    /// Recursively updates Level (depth) and Order (sibling index) for every node.
    /// </summary>
    private void RecalculateHierarchyMetadata()
    {
        for (int i = 0; i < RootNodes.Count; i++)
        {
            UpdateNodeMetadata(RootNodes[i], null, level: 0, order: i);
        }
    }
    
    private void UpdateNodeMetadata(TNode node, TNode? parent, int level, int order)
    {
        node.Parent = parent;
        node.ParentId = parent?.Id;
        node.Level = level;
        node.Order = order;

        for (int i = 0; i < node.Children.Count; i++)
        {
            UpdateNodeMetadata(node.Children[i], node, level + 1, order: i);
        }
    }
    
    
    #region Drag & Drop Operations

    public void HandleDragStart(TNode node) => DraggedNode = node;



    public void HandleDragOver(TNode node)
    {
        if (DraggedNode == null || DraggedNode == node || IsDescendant(DraggedNode, node.Id))
            return;

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
        
        

        targetNode.IsExpanded = true;

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

        var droppedNode = DraggedNode;

        UnlinkNode(DraggedNode);

        DraggedNode.Parent = null;
        DraggedNode.ParentId = null;

        if (!RootNodes.Contains(DraggedNode))
        {
            RootNodes.Add(DraggedNode);
        }

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

    #region Snapshot & Database Storage

    /// <summary>
    /// Generates a list of snapshot DTOs containing Id, ParentId, Level, and Order.
    /// </summary>
    public List<TreeNodeSnapshotDto> GetTreeSnapshot(Func<TNode, string> nameSelector)
    {
        return FlatNodes.Select(node => new TreeNodeSnapshotDto
        {
            Id = node.Id,
            ParentId = node.ParentId,
            Level = node.Level,
            Order = node.Order,
            Name = nameSelector(node)
        }).ToList();
    }

    /// <summary>
    /// Exports the snapshot list to a JSON string ready to be saved in the database.
    /// </summary>
    public string ExportSnapshotToJson(Func<TNode, string> nameSelector)
    {
        var snapshot = GetTreeSnapshot(nameSelector);
        return JsonSerializer.Serialize(snapshot, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    #endregion
    
    #region Search & Expansion Actions
    
    public void ApplyFilter(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            foreach (var node in FlatNodes) node.IsVisible = true;
        }
        else
        {
            string trimmed = query.Trim();
            foreach (var root in RootNodes) EvaluateVisibility(root, trimmed);
        }
        Refresh();
    }

    private bool EvaluateVisibility(TNode node, string query)
    {
        bool matches = node.MatchesQuery(query);
        bool hasVisibleChild = false;

        foreach (var child in node.Children)
        {
            if (EvaluateVisibility(child, query)) hasVisibleChild = true;
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
    
   #region Node Reordering (Strict Sibling Scope)

/// <summary>
/// Retrieves the direct sibling list for a given node (either RootNodes or parent's Children).
/// </summary>
public List<TNode> GetSiblings(TNode node)
{
    if (node == null) return new List<TNode>();
    return node.Parent != null ? node.Parent.Children : RootNodes;
}

/// <summary>
/// Up button is enabled ONLY if the node is NOT the first sibling among its level peers.
/// </summary>
public bool CanMoveUp(TNode node)
{
    if (node == null) return false;
    var siblings = GetSiblings(node);
    int index = siblings.IndexOf(node);
    return index > 0;
}

/// <summary>
/// Down button is enabled ONLY if the node is NOT the last sibling among its level peers.
/// </summary>
public bool CanMoveDown(TNode node)
{
    if (node == null) return false;
    var siblings = GetSiblings(node);
    int index = siblings.IndexOf(node);
    return index >= 0 && index < siblings.Count - 1;
}

/// <summary>
/// Swaps node with its preceding sibling under the same parent.
/// Moves the node and all of its sub-children as a complete unit.
/// </summary>
public void MoveNodeUp(TNode node)
{
    if (!CanMoveUp(node)) return;

    var siblings = GetSiblings(node);
    int index = siblings.IndexOf(node);

    if (index > 0)
    {
        // Swap sibling positions in adapter collection
        (siblings[index], siblings[index - 1]) = (siblings[index - 1], siblings[index]);

        // Synchronize underlying data model children
        node.Parent?.SyncChildDataOrder();

        // Recalculates Level and Order metadata across the hierarchy
        Refresh();
    }
}

/// <summary>
/// Swaps node with its succeeding sibling under the same parent.
/// Moves the node and all of its sub-children as a complete unit.
/// </summary>
public void MoveNodeDown(TNode node)
{
    if (!CanMoveDown(node)) return;

    var siblings = GetSiblings(node);
    int index = siblings.IndexOf(node);

    if (index >= 0 && index < siblings.Count - 1)
    {
        // Swap sibling positions in adapter collection
        (siblings[index], siblings[index + 1]) = (siblings[index + 1], siblings[index]);

        // Synchronize underlying data model children
        node.Parent?.SyncChildDataOrder();

        // Recalculates Level and Order metadata across the hierarchy
        Refresh();
    }
}

#endregion
}
