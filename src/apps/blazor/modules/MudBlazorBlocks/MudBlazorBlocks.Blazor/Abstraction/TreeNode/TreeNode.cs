using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction;

/// <summary>
/// Represents a generic hierarchical tree node for Blazor UI trees and virtualized components.
/// </summary>
/// <typeparam name="T">The type of payload data contained within the node.</typeparam>
public class TreeNode<T> : INotifyPropertyChanged where T : class, new()
{
    private T _value;
    private TreeNode<T>? _parent;
    private bool _isExpanded;
    private bool? _isChecked = false;
    private bool _isVisible = true;

    private readonly List<TreeNode<T>> _children = new();
    private readonly List<TreeNode<T>> _filteredChildren = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    #region Constructors

    public TreeNode()
    {
        _value = new T();
    }

    public TreeNode(T value, TreeNode<T>? parent = null)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
        if (parent != null)
        {
            parent.AddChildNode(this);
        }
    }

    #endregion

    #region Properties

    /// <summary>
    /// The underlying data payload carried by this node.
    /// </summary>
    public T Value
    {
        get => _value;
        set
        {
            if (!EqualityComparer<T>.Default.Equals(_value, value))
            {
                _value = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Reference to the parent node. Managing this automatically handles child list assignment.
    /// </summary>
    public TreeNode<T>? Parent
    {
        get => _parent;
        internal set
        {
            if (_parent != value)
            {
                _parent = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Level));
            }
        }
    }

    /// <summary>
    /// The complete list of child nodes under this node.
    /// </summary>
    public List<TreeNode<T>> Children => _children;

    /// <summary>
    /// Read-only view of filtered children used when tree filtering is active.
    /// </summary>
    public IReadOnlyList<TreeNode<T>> FilteredChildren => _filteredChildren;

    /// <summary>
    /// Indicates whether a filter is currently active on this node.
    /// </summary>
    public bool HasFilterActive => _filteredChildren.Count > 0;

    /// <summary>
    /// Gets the current child collection to render (returns FilteredChildren if filtered, otherwise Children).
    /// </summary>
    public IEnumerable<TreeNode<T>> RenderedChildren => HasFilterActive ? _filteredChildren : _children;

    /// <summary>
    /// Zero-based depth level of the node in the hierarchy (0 = Root).
    /// </summary>
    public int Level => Parent == null ? 0 : Parent.Level + 1;

    /// <summary>
    /// Indicates if this node has any underlying child nodes.
    /// </summary>
    public bool HasChildren => _children.Count > 0;

    /// <summary>
    /// Gets or sets whether this tree node is currently expanded in the UI.
    /// </summary>
    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (_isExpanded != value)
            {
                _isExpanded = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the check state of this node (true = checked, false = unchecked, null = indeterminate).
    /// </summary>
    public bool? IsChecked
    {
        get => _isChecked;
        set
        {
            if (_isChecked != value)
            {
                _isChecked = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets node visibility for custom UI filtering.
    /// </summary>
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            if (_isVisible != value)
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region Child Management Methods

    /// <summary>
    /// Creates a new child node wrapping the item and appends it to this node.
    /// </summary>
    public TreeNode<T> AddChild(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var childNode = new TreeNode<T>(item)
        {
            Parent = this
        };

        _children.Add(childNode);
        OnPropertyChanged(nameof(HasChildren));
        return childNode;
    }

    /// <summary>
    /// Adds an existing tree node as a child.
    /// </summary>
    public void AddChildNode(TreeNode<T> childNode)
    {
        ArgumentNullException.ThrowIfNull(childNode);

        if (childNode.Parent != null && childNode.Parent != this)
        {
            childNode.Parent.RemoveChild(childNode);
        }

        childNode.Parent = this;
        if (!_children.Contains(childNode))
        {
            _children.Add(childNode);
            OnPropertyChanged(nameof(HasChildren));
        }
    }

    /// <summary>
    /// Removes a specific child node.
    /// </summary>
    public bool RemoveChild(TreeNode<T> childNode)
    {
        ArgumentNullException.ThrowIfNull(childNode);

        if (_children.Remove(childNode))
        {
            childNode.Parent = null;
            _filteredChildren.Remove(childNode);
            OnPropertyChanged(nameof(HasChildren));
            return true;
        }

        return false;
    }

    /// <summary>
    /// Clears all children from this node.
    /// </summary>
    public void ClearChildren()
    {
        foreach (var child in _children)
        {
            child.Parent = null;
        }

        _children.Clear();
        _filteredChildren.Clear();
        OnPropertyChanged(nameof(HasChildren));
    }

    #endregion

    #region Filter Helper Methods

    /// <summary>
    /// Clears the filtered children list during recalculation.
    /// </summary>
    public void ClearFiltered()
    {
        _filteredChildren.Clear();
        OnPropertyChanged(nameof(HasFilterActive));
        OnPropertyChanged(nameof(RenderedChildren));
    }

    /// <summary>
    /// Adds a child to the filtered children list.
    /// </summary>
    public void AddToFiltered(TreeNode<T> child)
    {
        ArgumentNullException.ThrowIfNull(child);

        if (!_filteredChildren.Contains(child))
        {
            _filteredChildren.Add(child);
            OnPropertyChanged(nameof(HasFilterActive));
            OnPropertyChanged(nameof(RenderedChildren));
        }
    }

    #endregion

    #region INotifyPropertyChanged Implementation

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
