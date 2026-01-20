

using System.Reflection;
using FSH.Starter.BlazorShared;
using FSH.Starter.Shared.Authorization;

namespace FSH.Starter.Blazor.Modules;
public static class ModulesConstants
{
    public static readonly string ModulesGlobalNameSpace = typeof(ModulesConstants).Namespace!;
    public static readonly Dictionary<string, Assembly> RegisteredModules = new();
    public static readonly Dictionary<string, IBlazorModule> ModulesCache = new();
    public static readonly FshPermission[] AllModulesPermistions = [];
}
