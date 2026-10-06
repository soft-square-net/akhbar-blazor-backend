using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.Blazor.Modules.Keyword.Blazor;

internal static class Constants
{
    public const string ModuleName = "Keyword";
    public const string ModuleDisplayName = "Keywords";
    public static readonly string _content = $"_content/{typeof(Constants).Assembly.GetName().Name}";
    // public static readonly string _outdir =  $"{${outdir}}";
}
