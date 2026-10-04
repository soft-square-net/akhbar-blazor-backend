using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor;

internal static class Constants
{
    public const string ModuleName = "MudBlazorBlocks";
    public const string ModuleDisplayName = "Mud Blazor Blocks";
    public static readonly string _content = $"_content/{typeof(Constants).Assembly.GetName().Name}";
    // public static readonly string _outdir =  $"{${outdir}}";
}
