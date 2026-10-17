using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorModularEngine.Core.Models;

public class ModuleMetadata
{
    public string ModuleName { get; set; } = string.Empty;
    public string AssemblyUrl { get; set; } = string.Empty;
    public string Version { get; set; } = "1.0.0";
    public bool IsStartupModule { get; set; }
    public List<string> Dependencies { get; set; } = new();
}
