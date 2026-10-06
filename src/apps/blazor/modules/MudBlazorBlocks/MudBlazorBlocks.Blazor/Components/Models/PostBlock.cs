namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Components.Models;

public class PostBlock
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? ParentId { get; set; } // null = root block
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public BlockType Type { get; set; } = BlockType.Standard;
    public bool IsEditing { get; set; } = true;
    public List<PostBlock> Children { get; set; } = new();
    public int Order { get; set; } 
    // Helper method to add a child node
    public void AddChild(PostBlock child)
    {
        child.ParentId = Id;
        Children.Add(child);
    }
}

public enum BlockType
{
    Standard,
    Header,
    Callout,
    Quote
}
