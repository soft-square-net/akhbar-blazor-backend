using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services;
public interface IFileExplorerStateService
{
    public event Action OnChange;
    public event Action OnFileSelectionChanged;
    public event Action OnFileExplorerClearSelection;

    public void NotifyStateChanged();
    public void NotifyFileSelectionChanged();
    public void NotifyFileExplorerClearSelection();
}
