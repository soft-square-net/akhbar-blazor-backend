using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.Blazor.Modules;
public static class ModulesConstants
{
    public static readonly string ModulesGlobalNameSpace = typeof(ModulesConstants).Namespace!;
    public static readonly Dictionary<string, Assembly> RegisteredModules = new();
}
