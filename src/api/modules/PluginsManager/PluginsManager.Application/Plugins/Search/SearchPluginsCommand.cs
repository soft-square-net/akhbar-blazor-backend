using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.PluginsManager.Application.Plugins.Search;

public class SearchPluginsCommand() : PaginationFilter, IRequest<PagedList<SearchPluginsResponse>>
{
    
}
