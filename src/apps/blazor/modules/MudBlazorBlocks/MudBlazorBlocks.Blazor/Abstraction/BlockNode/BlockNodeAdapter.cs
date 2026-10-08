using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.TreeNode.Interfaces;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode;
public class BlockNodeAdapter : ITreeNodeItem<BlockNodeAdapter>
{
    public BlockNode Data { get; }

    public BlockNodeAdapter(BlockNode node)
    {
        Data = node;
        Children = new List<BlockNodeAdapter>();
    }

    public string Id => Data.Id;

    public string? ParentId
    {
        get => Data.ParentId;
        set { /* ParentId on BlockNode has a private setter */ }
    }

    public BlockNodeAdapter? Parent { get; set; }
    public List<BlockNodeAdapter> Children { get; set; }
    public bool IsExpanded { get; set; } = true;
    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// Factory method to build a full adapter hierarchy from a root BlockNode.
    /// </summary>
    public static BlockNodeAdapter FromBlockNode(BlockNode node, BlockNodeAdapter? parent = null)
    {
        var adapter = new BlockNodeAdapter(node)
        {
            Parent = parent
        };

        if (node.Children != null)
        {
            foreach (var child in node.Children)
            {
                adapter.Children.Add(FromBlockNode(child, adapter));
            }
        }

        return adapter;
    }
}
