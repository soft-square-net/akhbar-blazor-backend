namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.Toolbar;

public class ToolbarItem
{
    public ToolbarItemType Type { get; set; }
    public string? Icon { get; set; } // Stores the MudBlazor Icon string path
    public string? Tooltip { get; set; }
    public ToolbarItem Clone()
    {
        return new ToolbarItem
        {
            Type = this.Type,
            Icon = this.Icon,
            Tooltip = this.Tooltip
        };
    }
}
