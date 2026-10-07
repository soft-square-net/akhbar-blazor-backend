using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction;

public class TreeNode<T> where T : class, new()
{
    public T Value { get; set; }
    public TreeNode<T>? Parent { get; private set; }

    private readonly List<TreeNode<T>> _children = new();
    private readonly List<TreeNode<T>> _filtered = new();

    /// <summary>
    /// Indicates whether a filter has been actively applied to this node.
    /// </summary>
    public bool IsFiltered { get; private set; }
// UI State Flags
    public bool IsExpanded { get; set; }
    public bool IsSelected { get; set; }
    public bool? IsChecked { get; set; } // Nullable for tri-state checkboxes (Indeterminate)
    public bool IsDisabled { get; set; }
    
    // Calculates visual depth (Root = 0)
    public int Level => Parent == null ? 0 : Parent.Level + 1;
    
    // Check if the current node is a leaf
    public bool IsLeaf => Children.Count == 0;
    /// <summary>
    /// Returns filtered children if a filter is active, otherwise returns all children.
    /// </summary>
    public IReadOnlyList<TreeNode<T>> Children => IsFiltered ? _filtered : _children;

    public List<Expression<Func<TreeNode<T>, bool>>> Filters { get; } = new();

    public TreeNode(T value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    // --- Child Management ---

    public TreeNode<T> AddChild(T value)
    {
        var childNode = new TreeNode<T>(value) { Parent = this };
        _children.Add(childNode);
        return childNode;
    }

    public void AddChild(TreeNode<T> childNode)
    {
        ArgumentNullException.ThrowIfNull(childNode);
        childNode.Parent = this;
        _children.Add(childNode);
    }

    public bool RemoveChild(T value)
    {
        var childNode = _children.FirstOrDefault(c => EqualityComparer<T>.Default.Equals(c.Value, value));
        if (childNode is null) return false;

        childNode.Parent = null;
        return _children.Remove(childNode);
    }

    public bool RemoveChild(TreeNode<T> childNode)
    {
        if (_children.Remove(childNode))
        {
            childNode.Parent = null;
            return true;
        }
        return false;
    }

    // --- Filter Management ---

    public void AddToFiltered(TreeNode<T> childNode)
    {
        childNode.Parent = this;
        _filtered.Add(childNode);
        IsFiltered = true;
    }

    public void ClearFiltered()
    {
        _filtered.Clear();
        IsFiltered = false;
        foreach (var child in _children)
        {
            child.ClearFiltered();
        }
    }

    public IReadOnlyList<TreeNode<T>> ApplyFilters()
    {
        ClearFiltered();

        if (Filters.Count == 0)
        {
            return _children;
        }

        foreach (var filter in Filters)
        {
            TreeNodeUtility<T>.Filter(this, filter);
        }

        IsFiltered = true;
        return _filtered;
    }
}


// using System;
// using System.Collections.Generic;
// using System.Linq.Expressions;
//
// namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction;
//
// public class TreeNode<T> where T : class, new()
// {
//     public T Value { get; set; }
//     public TreeNode<T> Parent { get; private set; }
//     private List<TreeNode<T>> _children { get; } = new List<TreeNode<T>>();
//     public List<TreeNode<T>> _filtered { get; } = new List<TreeNode<T>>();
//     public List<TreeNode<T>> Children => _filtered.Count > 0? _filtered:_children;
//     public List<Expression<Func<TreeNode<T>, bool>>> Filters { get; } = new();
//     public TreeNode(T value)
//     {
//         Value = value;
//     }
//
//     public void AddChild(T value)
//     {
//         var childNode = new TreeNode<T>(value) { Parent = this };
//         _children.Add(childNode);
//     }
//     
//     public bool? RemoveChild(T value)
//     {
//         var childNode = _children.FirstOrDefault(c => c.Value.Equals(value));
//         if(childNode is not null) { _children.Remove(childNode); return true; }
//         return false;
//     }
//
//     public void AddChild(TreeNode<T> childNode)
//     {
//         childNode.Parent = this;
//         _children.Add(childNode);
//     }
//     public void RemoveChild(TreeNode<T> childNode)
//     {
//         childNode.Parent = null;
//         _children.Remove(childNode);
//     }
//     
//     public void AddToFiltered(T value)
//     {
//         var childNode = new TreeNode<T>(value) { Parent = this };
//         _filtered.Add(childNode);
//     }
//     
//     public void AddToFiltered(TreeNode<T> childNode)
//     {
//         childNode.Parent = this;
//         _filtered.Add(childNode);
//     }
//     
//     public void ClearFiltered() => _filtered.Clear();
//
//     private List<TreeNode<T>> Filter() 
//     {
//         _filtered.Clear(); 
//         foreach (var filter in Filters)
//         {
//             TreeNodeUtility<T>.Filter(this,filter); 
//             if(_filtered.Count == 0) break;
//         }
//         return _filtered;
//     }
// }
