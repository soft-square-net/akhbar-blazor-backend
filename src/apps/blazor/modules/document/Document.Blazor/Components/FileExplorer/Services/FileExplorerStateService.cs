using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services.Interfaces;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services;
public class FileExplorerStateService: IFileExplorerStateService
{
    public event Action OnChange;
    public event Action OnFileSelectionChanged;
    public event Action OnFileExplorerClearSelection;

    public void NotifyStateChanged() => OnChange?.Invoke();
    public void NotifyFileSelectionChanged() => OnFileSelectionChanged?.Invoke();

    public void NotifyFileExplorerClearSelection() => OnFileExplorerClearSelection?.Invoke();
}
