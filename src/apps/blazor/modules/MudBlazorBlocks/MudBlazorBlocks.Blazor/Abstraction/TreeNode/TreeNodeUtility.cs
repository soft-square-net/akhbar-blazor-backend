using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction;

public static class TreeNodeUtility<T> where T : class, new()
{
    /// <summary>
    /// Adds a collection of child items to the specified tree node.
    /// </summary>
    public static void AddChildren(TreeNode<T> treeNode, IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(treeNode);
        ArgumentNullException.ThrowIfNull(items);

        foreach (var item in items)
        {
            treeNode.AddChild(item);
        }
    }

    /// <summary>
    /// Clears all children from the specified tree node.
    /// </summary>
    public static void ClearChildren(TreeNode<T> treeNode)
    {
        ArgumentNullException.ThrowIfNull(treeNode);
        treeNode.Children.Clear();
    }

    /// <summary>
    /// Evaluates a filter expression across the tree and retains nodes matching the predicate 
    /// or having matching descendants in the filtered children collection.
    /// </summary>
    public static void Filter(TreeNode<T> rootNode, Expression<Func<TreeNode<T>, bool>> filterExpression)
    {
        ArgumentNullException.ThrowIfNull(rootNode);
        ArgumentNullException.ThrowIfNull(filterExpression);

        Func<TreeNode<T>, bool> predicate = filterExpression.Compile();

        rootNode.ClearFiltered();
        FilterRecursive(rootNode, predicate);
    }

    private static bool FilterRecursive(TreeNode<T> currentNode, Func<TreeNode<T>, bool> predicate)
    {
        bool selfMatches = predicate(currentNode);
        bool anyChildMatches = false;

        foreach (var child in currentNode.Children)
        {
            bool childHasMatch = FilterRecursive(child, predicate);
            if (childHasMatch)
            {
                currentNode.AddToFiltered(child);
                anyChildMatches = true;
            }
        }

        return selfMatches || anyChildMatches;
    }

    /// <summary>
    /// Flattened search returning all nodes matching the predicate across the subtree.
    /// </summary>
    public static List<TreeNode<T>> Where(TreeNode<T> treeNode, Expression<Func<TreeNode<T>, bool>> filterExpression)
    {
        ArgumentNullException.ThrowIfNull(treeNode);
        ArgumentNullException.ThrowIfNull(filterExpression);

        Func<TreeNode<T>, bool> predicate = filterExpression.Compile();
        List<TreeNode<T>> result = new();

        WhereRecursive(treeNode, predicate, result);
        return result;
    }

    private static void WhereRecursive(TreeNode<T> currentNode, Func<TreeNode<T>, bool> predicate, List<TreeNode<T>> result)
    {
        if (predicate(currentNode))
        {
            result.Add(currentNode);
        }

        foreach (var child in currentNode.Children)
        {
            WhereRecursive(child, predicate, result);
        }
    }

    /// <summary>
    /// Traverses up from the given node and yields all ancestor values up to the root.
    /// </summary>
    public static IEnumerable<T> GetAncestors(TreeNode<T> currentNode)
    {
        ArgumentNullException.ThrowIfNull(currentNode);

        var walker = currentNode.Parent;
        while (walker != null)
        {
            yield return walker.Value;
            walker = walker.Parent;
        }
    }

    /// <summary>
    /// Flattens and traverses the tree using Breadth-First Search (BFS).
    /// </summary>
    public static IEnumerable<TreeNode<T>> TraverseBreadthFirst(TreeNode<T> root)
    {
        ArgumentNullException.ThrowIfNull(root);

        var queue = new Queue<TreeNode<T>>();
        queue.Enqueue(root);

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

    /// <summary>
    /// Finds the first node matching the specified predicate using Depth-First Search (DFS).
    /// </summary>
    public static TreeNode<T>? FindNode(TreeNode<T> root, Func<TreeNode<T>, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(root);
        ArgumentNullException.ThrowIfNull(predicate);

        if (predicate(root)) return root;

        foreach (var child in root.Children)
        {
            var result = FindNode(child, predicate);
            if (result != null) return result;
        }

        return null;
    }

    /// <summary>
    /// Recursively sorts node children in-place based on a key selector.
    /// </summary>
    public static void SortRecursive<TKey>(TreeNode<T> node, Func<T, TKey> keySelector, bool descending = false)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(keySelector);

        List<TreeNode<T>> sortedChildren = descending
            ? node.Children.OrderByDescending(c => keySelector(c.Value)).ToList()
            : node.Children.OrderBy(c => keySelector(c.Value)).ToList();

        node.Children.Clear();
        foreach (var child in sortedChildren)
        {
            node.Children.Add(child);
            SortRecursive(child, keySelector, descending);
        }
    }

    /// <summary>
    /// Flattens the tree into a list containing only expanded nodes (ideal for virtualized lists).
    /// </summary>
    public static List<TreeNode<T>> ToVisibleFlatList(TreeNode<T> root)
    {
        ArgumentNullException.ThrowIfNull(root);

        var result = new List<TreeNode<T>>();
        FlattenVisibleRecursive(root, result);
        return result;
    }

    private static void FlattenVisibleRecursive(TreeNode<T> current, List<TreeNode<T>> result)
    {
        result.Add(current);

        if (current.IsExpanded)
        {
            foreach (var child in current.Children)
            {
                FlattenVisibleRecursive(child, result);
            }
        }
    }

    /// <summary>
    /// Sets the check state for a node, optionally cascading down to children and recalculating parent state up the chain.
    /// </summary>
    public static void SetCheckedState(TreeNode<T> node, bool isChecked, bool cascadeToChildren = true)
    {
        ArgumentNullException.ThrowIfNull(node);

        node.IsChecked = isChecked;

        if (cascadeToChildren)
        {
            foreach (var child in node.Children)
            {
                SetCheckedState(child, isChecked, cascadeToChildren: true);
            }
        }

        UpdateParentCheckState(node.Parent);
    }

    private static void UpdateParentCheckState(TreeNode<T>? parent)
    {
        if (parent == null) return;

        int checkedCount = parent.Children.Count(c => c.IsChecked == true);
        int uncheckedCount = parent.Children.Count(c => c.IsChecked == false);
        int totalCount = parent.Children.Count;

        if (checkedCount == totalCount)
            parent.IsChecked = true;
        else if (uncheckedCount == totalCount)
            parent.IsChecked = false;
        else
            parent.IsChecked = null; // Indeterminate state

        UpdateParentCheckState(parent.Parent);
    }
}
