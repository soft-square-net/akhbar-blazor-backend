using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Components.Models;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Components.Abstractions;

public class PostBlockTreeDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public BlockType Type { get; set; }
    public List<PostBlockTreeDto> Children { get; set; } = new();
    
    public List<PostBlockTreeDto> ExportTree(List<PostBlock> flatBlocks, string? parentId = null)
    {
        return flatBlocks
            // .Where(b => b.ParentId == parentId)
            // .OrderBy(b => b.Order)
            .Select(b => new PostBlockTreeDto
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                Type = b.Type,
                Children = ExportTree(flatBlocks, b.Id)
            })
            .ToList();
    }
}
