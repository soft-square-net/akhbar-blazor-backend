using System.Reflection;
using System.Runtime.Loader;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.PluginsManager.Application.Plugins.Search;
using FSH.Starter.WebApi.PluginsManager.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.FSHPlugin;

namespace FSH.Starter.WebApi.PluginsManager.Application.Brands.Search.v1;

public sealed class SearchBrandsHandler(
    [FromKeyedServices("plugin:plugins")] IReadRepository<Plugin> repository)
    : IRequestHandler<SearchPluginsCommand, PagedList<PluginModelDto>>
{
    public async Task<PagedList<PluginModelDto>> Handle(SearchPluginsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        // var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var items = new List<PluginModelDto>(){
                        new (){
                        Id=Guid.NewGuid().ToString(),
                        Name="Document.Blazor",
                        // AssemblyName="FSH.Starter.Blazor.Modules.Document.Blazor",
                        PluginFileName = "FSH.Starter.Blazor.Modules.Document.Blazor",
                        DllUrl = "https://www.fsh.starter.blazor.modules/",
                        IsEnabled =  false,
                        IsStartup = false,
                        IsPubished = false,
                    },
                    new (){
                        Id=Guid.NewGuid().ToString(),
                        Name="ElsaWorkflow.Blazor",
                        // AssemblyName="FSH.Starter.Blazor.Modules.ElsaWorkflow.Blazor",
                        PluginFileName = "FSH.Starter.Blazor.Modules.ElsaWorkflow.Blazor",
                        DllUrl = "https://www.fsh.starter.blazor.modules/",
                        IsEnabled =  false,
                        IsStartup = false,
                        IsPubished = false,
                    },
                    new (){
                        Id=Guid.NewGuid().ToString(),
                        Name="FSHeroLayout.Blazor",
                        AssemblyData = await LoadAssemlby("FSH.Starter.Blazor.Modules.FSHeroLayout.Blazor", cancellationToken) ,
                        PluginFileName = "FSH.Starter.Blazor.Modules.FSHeroLayout.Blazor",
                        DllUrl = $"_content/FSH.Starter.Blazor.Modules.FSHeroLayout.Blazor",
                        IsEnabled =  true,
                        IsStartup = true,
                        IsPubished = false,
                    },
                    new (){
                        Id=Guid.NewGuid().ToString(),
                        Name="MudBlazorBlocks.Blazor",
                        //AssemblyName="FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor",
                        AssemblyData = await LoadAssemlby("FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor", cancellationToken) ,
                        PluginFileName = "FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor",
                        DllUrl = "https://www.fsh.starter.blazor.modules/",
                        IsEnabled =  true,
                        IsStartup = true,
                        IsPubished = false,
                    },
                    new (){
                        Id=Guid.NewGuid().ToString(),
                        Name="Keyword.Blazor",
                        //AssemblyName="FSH.Starter.Blazor.Modules.Keyword.Blazor",
                        PluginFileName = "FSH.Starter.Blazor.Modules.Keyword.Blazor",
                        DllUrl = "https://www.fsh.starter.blazor.modules/",
                        IsEnabled =  false,
                        IsStartup = false,
                        IsPubished = false,
                    }
        };
        var totalCount = items.Count();

        return new PagedList<PluginModelDto>(items, request!.PageNumber, request!.PageSize, totalCount);
    }

    private async Task<Byte[]> LoadAssemlby(string PluginFileName, CancellationToken cancellationToken)
    {
        // 1. Resolve the path relative to the execution root (bin/ebug or Published directory)
        string baseDir = AppContext.BaseDirectory;

        // 2. Combine the paths to target rootfolder/assets/plugins/
        string pluginPath = Path.Combine(baseDir, "assets", "plugins", $"{PluginFileName}.dll");

        // Validate that the file actually exists
        if (!File.Exists(pluginPath))
        {
            // throw new FileNotFoundException($"Plugin assembly not found at path: {pluginPath}");
            return null;
        }

        // 3. Load the assembly using the Default AssemblyLoadContext
        // (Ensures dependency resolution behaves predictably)
        // Assembly pluginAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(pluginPath);
        byte[] fileBytes = await File.ReadAllBytesAsync(pluginPath, cancellationToken);

        // Optional: Interact with types from the assembly
        // var types = pluginAssembly.GetTypes();

        return fileBytes;
    }
}
