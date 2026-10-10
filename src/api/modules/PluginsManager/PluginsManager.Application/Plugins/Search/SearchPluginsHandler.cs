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
    : IRequestHandler<SearchPluginsCommand, PagedList<FSHPluginResponse>>
{
    public async Task<PagedList<FSHPluginResponse>> Handle(SearchPluginsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        // var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var items = new List<FSHPluginResponse>(){ 
                        new (){
                        Id=Guid.NewGuid().ToString(),
                        Name="Document.Blazor",
                        AssemblyName="FSH.Starter.Blazor.Modules.Document.Blazor",
                        DllUrl = "https://www.fsh.starter.blazor.modules/",
                        IsEnabled =  true
                    },
                    new (){
                        Id=Guid.NewGuid().ToString(),
                        Name="ElsaWorkflow.Blazor",
                        AssemblyName="FSH.Starter.Blazor.Modules.ElsaWorkflow.Blazor",
                        DllUrl = "https://www.fsh.starter.blazor.modules/",
                        IsEnabled =  true
                    },
                    new (){
                        Id=Guid.NewGuid().ToString(),
                        Name="FSHeroLayout.Blazor",
                        AssemblyName="FSH.Starter.Blazor.Modules.FSHeroLayout.Blazor",
                        DllUrl = "https://www.fsh.starter.blazor.modules/",
                        IsEnabled =  true
                    },
                    new (){
                        Id=Guid.NewGuid().ToString(),
                        Name="MudBlazorBlocks.Blazor",
                        AssemblyName="FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor",
                        DllUrl = "https://www.fsh.starter.blazor.modules/",
                        IsEnabled =  true
                    },
                    new (){
                        Id=Guid.NewGuid().ToString(),
                        Name="MudBlazorBlocks.Blazor",
                        AssemblyName="FSH.Starter.Blazor.Modules.Keyword.Blazor",
                        DllUrl = "https://www.fsh.starter.blazor.modules/",
                        IsEnabled =  true
                    }
        };
        var totalCount = items.Count();

        return new PagedList<FSHPluginResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
