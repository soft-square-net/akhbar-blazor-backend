using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.BlazorShared.Services;

public class ContextMenuService
{
    public event Action<double, double, object>? OnShow;
    public event Action? OnHide;

    public void Show(double x, double y, object targetItem)
    {
        OnShow?.Invoke(x, y, targetItem);
    }

    public void Hide()
    {
        OnHide?.Invoke();
    }
}
