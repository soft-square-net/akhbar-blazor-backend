using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Components.Grid;

public class GridEditorState
{
    public GridBlockItem? DraggedItem {get; private set; }
    public GridBlockItem? SelectedItem {get; private set; }

    public event Action? OnStateChanged;

    public void StartDrag(GridBlockItem item)
    {
        DraggedItem = item;
    }

    public void ClearDrag()
    {
        DraggedItem = null;
    }

    public void SelectItem(GridBlockItem item)
    {
        SelectedItem = item;
        NotifyStateChanged();
    }

    public void NotifyStateChanged() => OnStateChanged?.Invoke();
}
