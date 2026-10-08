namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.TreeNode;

public class TreeNodeSnapshotDto
{
    public string Id { get; set; } = string.Empty;
    public string? ParentId { get; set; }
    public int Level { get; set; }
    public int Order { get; set; }
    public string Name { get; set; } = string.Empty;
}
