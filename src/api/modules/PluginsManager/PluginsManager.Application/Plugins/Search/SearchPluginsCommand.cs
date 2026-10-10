using FSH.Framework.Core.Paging;
using MediatR;
using Shared.FSHPlugin;

namespace FSH.Starter.WebApi.PluginsManager.Application.Plugins.Search;

public class SearchPluginsCommand() : PaginationFilter, IRequest<PagedList<FSHPluginResponse>>
{
    
}
