using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.TreeNode.Interfaces;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode;
public class BlockNodeAdapter : ITreeNodeAdapter<BlockNodeAdapter> 
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? ParentId { get; set; }
    public BlockNodeAdapter? Parent { get; set; }
    public List<BlockNodeAdapter> Children { get; set; } = new();
    public BlockNode Data { get; set; } = new();

    public int Level { get; set; }
    public int Order { get; set; }
    
    public bool IsExpanded { get; set; } = true;
    public bool IsVisible { get; set; } = true;

    public bool MatchesQuery(string query) =>
        Data.Name != null && Data.Name.Contains(query, StringComparison.OrdinalIgnoreCase);

    public void AddChildData(BlockNodeAdapter childNode)
    {
        if (!Data.Children.Contains(childNode.Data))
            Data.Children.Add(childNode.Data);
    }

    public void RemoveChildData(BlockNodeAdapter childNode)
    {
        Data.Children.Remove(childNode.Data);
    }
    
    public void SyncChildDataOrder()
    {
        Data.Children = Children.Select(c => c.Data).ToList();
    }

    public static BlockNodeAdapter FromBlockNode(BlockNode blockNode, BlockNodeAdapter? parent = null)
    {
        var adapter = new BlockNodeAdapter
        {
            Data = blockNode,
            Parent = parent,
            ParentId = parent?.Id
        };

        if (blockNode.Children != null)
        {
            foreach (var child in blockNode.Children)
            {
                adapter.Children.Add(FromBlockNode(child, adapter));
            }
        }

        return adapter;
    }

    private static readonly JsonSerializerOptions _toJsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };
    private static readonly JsonSerializerOptions _fromJsonOptions = new() {PropertyNameCaseInsensitive = true };
    
    public static string ToJson(BlockNodeAdapter adapter)
    {
        return JsonSerializer.Serialize(adapter, _toJsonOptions);
    }
    
    public static BlockNodeAdapter? FromJson(string adapter)
    {
        return JsonSerializer.Deserialize<BlockNodeAdapter>(adapter, _fromJsonOptions);
    }
}
