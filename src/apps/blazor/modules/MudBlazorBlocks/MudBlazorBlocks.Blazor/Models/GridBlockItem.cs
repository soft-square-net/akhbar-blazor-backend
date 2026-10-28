using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Models;

public class GridBlockItem
{
    public string Id { get; set; } = "block_" + Guid.NewGuid().ToString("n").Substring(0, 9);
    public string Type { get; set; } // "Container", "Heading", "Paragraph", "Image", "CustomHtml"
    public int ColSpan { get; set; } = 6;
    public int RowSpan { get; set; } = 1;
    public string Content { get; set; } = string.Empty;
    public string TagName { get; set; } = "p";
    public string ImageUrl { get; set; } = "https://images.unsplash.com/photo-1618005182384-a83a8bd57fbe?auto=format&fit=crop&w=600&q=80";
    public string CustomCss { get; set; } = string.Empty;
    public List<GridBlockItem> Children { get; set; } = new();

    public GridBlockItem() { }

    public GridBlockItem(string type, int colSpan = 6, int rowSpan = 1, string content = "")
    {
        Type = type;
        ColSpan = colSpan;
        RowSpan = rowSpan;
        TagName = type == "Heading" ? "h2" : "p";
        Content = !string.IsNullOrEmpty(content) ? content : GetDefaultContent(type);
    }

    private string GetDefaultContent(string type)
    {
        return type switch
        {
            "Heading" => "WYSIWYG CSS Grid Block",
            "Paragraph" => "Toolbars float on top of blocks on hover/focus without causing layout shift or height expansion!",
            "CustomHtml" => "<button style=\"background:#594AE2; color:white; padding:8px 16px; border:none; border-radius:6px; font-weight:bold; cursor:pointer;\">Custom Action Button</button>",
            _ => string.Empty
        };
    }
}
