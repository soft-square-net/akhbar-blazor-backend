using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Interfaces;
using MudBlazor;
using Nextended.Core.Extensions;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Helpers;

public static class TreeViewHelper<TDataModel> where TDataModel : IExplorerItemModel
{
    public static bool GetById(IReadOnlyCollection<ITreeItemData<TDataModel>> items, Guid Id, out ITreeItemData<TDataModel> matchedNode)
    {
        matchedNode = null!;
        foreach (var item in items)
        {
            // Match rule
            if (item.Value != null && item.Value.Id == Id)
            {
                item.Selected = true;
                matchedNode = item;
                return true;
            }

            // Recursive step
            if (item.Children != null && item.Children.Any())
            {
                if (GetById(item.Children, Id, out matchedNode))
                {
                    item.Expanded = true; // Expand parents from bottom up
                    return true;
                }
            }
        }
        return false;
    }
    public static bool GetByIdAndSelect(IReadOnlyCollection<ITreeItemData<TDataModel>> items, Guid Id, out ITreeItemData<TDataModel> matchedNode)
    {
        matchedNode = null!;
        foreach (var item in items)
        {
            // Match rule
            if (item.Value != null && item.Value.Id == Id)
            {
                item.Selected = true;
                matchedNode = item;
                return true;
            }

            // Recursive step
            if (item.Children != null && item.Children.Any())
            {
                if (GetByIdAndSelect(item.Children, Id, out matchedNode))
                {
                    item.Expanded = true; // Expand parents from bottom up
                    return true;
                }
            }
        }
        return false;
    }
    public static bool GetByIdAndSelect(IReadOnlyCollection<TDataModel> items, Guid Id, out TDataModel? matchedNode)
    {
        matchedNode = default!;
        foreach (var item in items)
        {
            // Match rule
            if (item != null && item.Id == Id)
            {
                matchedNode = item;
                return true;
            }

            // Recursive step
            if (item.Children != null && item.Children.Any())
            {
                if (GetByIdAndSelect(item.Children as ReadOnlyCollection<TDataModel>, Id, out matchedNode))
                {
                    item.SetExposed(); // Expand parents from bottom up
                    return true;
                }
            }
        }
        return false;
    }
    private static bool SearchAndSelectSingle(IReadOnlyCollection<ITreeItemData<TDataModel>> items, string targetText, out ITreeItemData<TDataModel> matchedNode)
    {
        matchedNode = null!;
        foreach (var item in items)
        {
            // Match rule
            if (item.Value != null && item.Value.Name.Contains(targetText, StringComparison.OrdinalIgnoreCase))
            {
                item.Selected = true;
                matchedNode = item;
                return true;
            }

            // Recursive step
            if (item.Children != null && item.Children.Any())
            {
                if (SearchAndSelectSingle(item.Children, targetText, out matchedNode))
                {
                    item.Expanded = true; // Expand parents from bottom up
                    return true;
                }
            }
        }
        return false;
    }

    private static bool SearchAndSelectMultiple(IReadOnlyCollection<ITreeItemData<TDataModel>> items, string targetText)
    {
        bool branchHasMatch = false;

        foreach (var item in items)
        {
            bool currentItemMatches = false;

            if (item.Value != null && item.Value.Name.Contains(targetText, StringComparison.OrdinalIgnoreCase))
            {
                item.Selected = true;
                currentItemMatches = true;
            }

            // Always check children even if parent matches (collecting all levels)
            bool childrenHaveMatch = false;
            if (item.Children != null && item.Children.Any())
            {
                childrenHaveMatch = SearchAndSelectMultiple(item.Children, targetText);
            }

            // Expand node if this item or any nested children match
            if (currentItemMatches || childrenHaveMatch)
            {
                item.Expanded = true;
                branchHasMatch = true;
            }
        }

        return branchHasMatch;
    }

    public static void ClearSelectionAndExpansion(IReadOnlyCollection<ITreeItemData<TDataModel>> items)
    {
        foreach (var item in items)
        {
            item.Selected = false;
            item.Expanded = false;
            if (item.Children != null) ClearSelectionAndExpansion(item.Children);
        }
    }


    public static List<ITreeItemData<TDataModel>> MapNodesToTreeItemData(List<TDataModel>? nodes)
    {
        if (nodes == null || !nodes.Any())
            return new List<ITreeItemData<TDataModel>>();
        return nodes.Select(node => new TreeItemData<TDataModel>
        {
            Text = node.Name,
            Value = node,
            Children = node.Children != null && node.Children.Any()
                ? MapNodesToTreeItemData(node.Children as List<TDataModel>)
                : null
        }).ToList<ITreeItemData<TDataModel>>();
    }
}
