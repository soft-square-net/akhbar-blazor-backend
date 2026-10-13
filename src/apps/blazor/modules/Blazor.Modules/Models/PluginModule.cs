using System.Reflection;

public class PluginModule
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string DllUrl { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public Assembly? LoadedAssembly { get; set; }

    // Path to the CSS Isolation bundle in Blazor
    public string CssBundlePath => $"_content/{AssemblyName}/{AssemblyName}.bundle.scp.css";

    public string AssemblyName => LoadedAssembly?.GetName().Name ?? Id;
}
