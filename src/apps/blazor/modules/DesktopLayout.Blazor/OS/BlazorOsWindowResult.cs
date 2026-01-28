using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudBlazor;

namespace FSH.Starter.Blazor.Modules.DesktopLayout.Blazor.OS;
public class BlazorOsWindowResult : DialogResult
{
    protected internal BlazorOsWindowResult(object? data, Type? resultType, bool canceled) : base(data, resultType, canceled)
    {
    }
}
