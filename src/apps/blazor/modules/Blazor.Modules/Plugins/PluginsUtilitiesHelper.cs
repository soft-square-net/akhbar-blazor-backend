using System.Collections.ObjectModel;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Shared.FSHPlugin;

namespace FSH.Starter.Blazor.Modules.Plugins;

public static class PluginsUtilitiesHelper
{
    public static List<PluginModelDto>  GetPluginInfoFromConfiguration(WebAssemblyHostBuilder builder)
    {

        string? jsonString = builder.Configuration.GetValue<string>("Plugins");
        return 
            JsonSerializer.Deserialize<Collection<PluginModelDto>>(
                jsonString,new JsonSerializerOptions(){PropertyNameCaseInsensitive = true}).ToList()
            ?? new List<PluginModelDto>();
    }

}
