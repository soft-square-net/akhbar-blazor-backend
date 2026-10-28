
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Models;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Services;

public class GridDesignerState
{
    public GridBlockItem RootContainer { get; set; } = new("Container", 12, 1);
    public GridBlockItem? SelectedBlock { get; set; }
    public GridBlockItem? DraggedBlock { get; set; }

    public event Action? OnStateChanged;

    public void NotifyStateChanged() => OnStateChanged?.Invoke();

    public void InitDemoData()
    {
        RootContainer.Children.Clear();

        var header = new GridBlockItem("Heading", 12, 1, "Absolute Overlay Toolbars & Auto-Scaling RowSpans")
        {
            TagName = "h1"
        };
        RootContainer.Children.Add(header);

        var gridSection = new GridBlockItem("Container", 12, 1);
        var subHeading = new GridBlockItem("Heading", 7, 1, "Dynamic Container Resizing & Zero Shift")
        {
            TagName = "h3"
        };
        var paragraph = new GridBlockItem("Paragraph", 7, 2, "Toolbars now float directly over block content using absolute positioning. Hovering or selecting a block will never cause layout shifts or height expansion!");
        var imgBlock = new GridBlockItem("Image", 5, 3);

        gridSection.Children.Add(subHeading);
        gridSection.Children.Add(paragraph);
        gridSection.Children.Add(imgBlock);
        RootContainer.Children.Add(gridSection);

        var customBlock = new GridBlockItem("CustomHtml", 12, 1);
        RootContainer.Children.Add(customBlock);

        SelectedBlock = header;
        RecalculateAllContainerRowSpans();
        NotifyStateChanged();
    }

    public (GridBlockItem? Item, GridBlockItem? Parent) FindBlockById(string id, GridBlockItem? current = null)
    {
        current ??= RootContainer;
        if (current.Id == id) return (current, null);

        foreach (var child in current.Children)
        {
            if (child.Id == id) return (child, current);
            if (child.Type == "Container")
            {
                var found = FindBlockById(id, child);
                if (found.Item != null) return found;
            }
        }
        return (null, null);
    }

    public bool RemoveBlock(string id)
    {
        var (item, parent) = FindBlockById(id);
        if (item != null && parent != null)
        {
            if (parent.Children.Remove(item))
            {
                if (SelectedBlock?.Id == id) SelectedBlock = null;
                RecalculateAllContainerRowSpans();
                NotifyStateChanged();
                return true;
            }
        }
        return false;
    }

    public void RecalculateContainerRowSpan(GridBlockItem container)
    {
        if (container == null || container.Type != "Container") return;

        foreach (var child in container.Children)
        {
            if (child.Type == "Container") RecalculateContainerRowSpan(child);
        }

        if (!container.Children.Any())
        {
            container.RowSpan = 1;
            return;
        }

        int currentCol = 0;
        int estimatedRows = 0;
        int maxRowInCurrentLine = 0;

        foreach (var child in container.Children)
        {
            int childCols = Math.Min(12, Math.Max(1, child.ColSpan));
            int childRows = Math.Max(1, child.RowSpan);

            if (currentCol + childCols > 12)
            {
                estimatedRows += maxRowInCurrentLine;
                currentCol = childCols;
                maxRowInCurrentLine = childRows;
            }
            else
            {
                currentCol += childCols;
                maxRowInCurrentLine = Math.Max(maxRowInCurrentLine, childRows);
            }
        }

        estimatedRows += maxRowInCurrentLine;
        container.RowSpan = Math.Max(1, estimatedRows);
    }

    public void RecalculateAllContainerRowSpans()
    {
        RecalculateContainerRowSpan(RootContainer);
    }
}
