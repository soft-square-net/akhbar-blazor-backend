using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.BlazorShared;
internal static class Constants
{
    public const string ModuleName = "BlazorShared";
    public static readonly string _content = $"_content/{typeof(Constants).Assembly.GetName().Name}";

}
