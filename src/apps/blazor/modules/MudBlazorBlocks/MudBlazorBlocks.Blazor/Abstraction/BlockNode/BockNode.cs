using System.Text.Json;
using System.Text.Json.Serialization;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode.Inputs;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.Toolbar;
using Nextended.Core.DeepClone;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode;

public class BlockNode
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public List<BlockNodeInput> Inputs { get; set; } = new();

    public List<ToolbarItem> ToolbarItems { get; set; } = new();

    public string ParentId { get; private set; }
    public BlockNode? Parent { get; set; }

    public List<BlockNode> Children { get; set; } = new();

    public Dictionary<string, string> JsonViews { get; set; } = new();
    public string CurrentJsonViews { get; set; }

    public bool IsRoot => Parent is null;

    public string ToJson()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        return JsonSerializer.Serialize(this, options);
    }

    public static BlockNode? FromJson(string json)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<BlockNode>(json, options);
    }

    public BlockNode DeepClone()
    {
        return new BlockNode
        {
            Id = this.Id,
            Name = this.Name,
            Inputs = this.Inputs.Select(i => i.Clone()).ToList(),
            ToolbarItems = this.ToolbarItems.Select(i => i.Clone()).ToList(),
            ParentId = this.ParentId,
            Parent = this.Parent,
            CurrentJsonViews = this.CurrentJsonViews,
            Children = this.Children?.Select(i => i.DeepClone())?.ToList() ?? new(),
            JsonViews = this.JsonViews.CloneDeep(),
        };
    }
}
