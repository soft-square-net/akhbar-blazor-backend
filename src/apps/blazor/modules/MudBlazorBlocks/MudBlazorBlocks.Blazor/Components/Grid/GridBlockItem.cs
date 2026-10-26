using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Components.Grid;


public enum BlockType
{
    Container,
    Heading,
    Text,
    Image,
    CustomHtml
}

public class GridBlockItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public BlockType Type { get; set; } = BlockType.Text;

    // Grid Spans (1 to 12 layout)
    public int ColSpan { get; set; } = 6; // Default width out of 12 columns
    public int RowSpan { get; set; } = 1; // Row height span

    // Content Data
    public string Content { get; set; } = "Sample Content";
    public string TagName { get; set; } = "h2"; // For headings/text
    public string ImageUrl { get; set; } = "https://via.placeholder.com/300x150";
    public string CustomCss { get; set; } = "";

    // Recursive Structure
    public List<GridBlockItem> Children { get; set; } = new();

    // Export to HTML WYSIWYG Output
    public string RenderToHtml()
    {
        var style = $"grid-column: span {ColSpan}; grid-row: span {RowSpan}; {CustomCss}";

        if (Type == BlockType.Container)
        {
            var innerHtml = string.Join("", Children.Select(c => c.RenderToHtml()));
            return $"<div style=\"display: grid; grid-template-columns: repeat(12, 1fr); gap: 16px; {style}\">{innerHtml}</div>";
        }

        return Type switch
        {
            BlockType.Heading => $"<{TagName} style=\"{style}\">{Content}</{TagName}>",
            BlockType.Text => $"<p style=\"{style}\">{Content}</p>",
            BlockType.Image => $"<img src=\"{ImageUrl}\" alt=\"\" style=\"width:100%; height:100%; object-fit:cover; {style}\"/>",
            BlockType.CustomHtml => $"<div style=\"{style}\">{Content}</div>",
            _ => ""
        };
    }
}
