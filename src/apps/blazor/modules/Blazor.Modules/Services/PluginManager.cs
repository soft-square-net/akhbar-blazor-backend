// Services/PluginManager.cs
using System.Net.Http;
using System.Reflection;

public class PluginManager
{
    private readonly HttpClient _httpClient;
    private readonly List<PluginModule> _plugins = new();

    public event Action? OnPluginsChanged;

    public IReadOnlyList<PluginModule> Plugins => _plugins.AsReadOnly();

    public PluginManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Loads a plugin dynamically over HTTP.
    /// </summary>
    public async Task LoadPluginAsync(string id, string name, string dllRelativeUrl, bool enabledByDefault = true)
    {
        var existing = _plugins.FirstOrDefault(p => p.Id == id);
        if (existing != null) return;

        var plugin = new PluginModule
        {
            Id = id,
            Name = name,
            DllUrl = dllRelativeUrl,
            IsEnabled = enabledByDefault
        };

        if (plugin.IsEnabled)
        {
            await FetchAndLoadAssemblyAsync(plugin);
        }

        _plugins.Add(plugin);
        OnPluginsChanged?.Invoke();
    }

    public async Task TogglePluginAsync(string id, bool enable)
    {
        var plugin = _plugins.FirstOrDefault(p => p.Id == id);
        if (plugin == null) return;

        plugin.IsEnabled = enable;

        if (enable && plugin.LoadedAssembly == null)
        {
            await FetchAndLoadAssemblyAsync(plugin);
        }

        OnPluginsChanged?.Invoke();
    }

    private async Task FetchAndLoadAssemblyAsync(PluginModule plugin)
    {
        try
        {
            // Fetch DLL bytes over HTTP
            byte[] bytes = await _httpClient.GetByteArrayAsync(plugin.DllUrl);

            // Load assembly into WebAssembly memory context
            plugin.LoadedAssembly = Assembly.Load(bytes);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load plugin {plugin.Name}: {ex.Message}");
            plugin.IsEnabled = false;
        }
    }

    public IEnumerable<Assembly> GetActiveAssemblies()
    {
        return _plugins
            .Where(p => p.IsEnabled && p.LoadedAssembly != null)
            .Select(p => p.LoadedAssembly!);
    }
}
