using System.Reflection;

namespace Shared.FSHPlugin;

public record FSHPluginResponse
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string DllUrl { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public string AssemblyName  { get; set; } = string.Empty;
    // Path to the CSS Isolation bundle in Blazor
    public string CssBundlePath => $"_content/{AssemblyName}/{AssemblyName}.bundle.scp.css";
}
