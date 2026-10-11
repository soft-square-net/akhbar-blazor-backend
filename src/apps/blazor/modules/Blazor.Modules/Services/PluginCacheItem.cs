using Elsa.Features.Services;
using Shared.FSHPlugin;

namespace FSH.Starter.Blazor.Modules.Services;

public class PluginCacheItem
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string DllUrl { get; set; } = string.Empty;
    public bool IsStartup { get; set; }        // True = Load on boot, False = Lazy load on demand
    public bool IsEnabled { get; set; }
    public bool IsLoaded{ get; set; }
    public bool IsPubished { get; set; }       // True = Skip PluginUtilities Load in Development mode because file not exist  
    public decimal Version { get; set; }
    public int weight { get; set; }
    
    // Runtime properties
    public IModule? Instance { get; set; }
    public string PluginFullName { get; set; } = default!;
    public byte[] AssemblyData { get; set; } = default!;

    public string AssemblyFullName => PluginFullName; 
    public string CssBundlePath => $"_content/{PluginFullName}/{PluginFullName}.bundle.scp.css";
    public List<string> Dependencies { get; set; } = [];  // To Check&Load List  dependant assemblys FullName .
}
