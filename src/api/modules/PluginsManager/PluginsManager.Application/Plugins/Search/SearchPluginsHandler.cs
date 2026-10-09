using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.PluginsManager.Application.Plugins.Search;
using FSH.Starter.WebApi.PluginsManager.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.PluginsManager.Application.Brands.Search.v1;

public sealed class SearchBrandsHandler(
    [FromKeyedServices("plugin:plugins")] IReadRepository<Plugin> repository)
    : IRequestHandler<SearchPluginsCommand, PagedList<SearchPluginsResponse>>
{
    public async Task<PagedList<SearchPluginsResponse>> Handle(SearchPluginsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        // var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var items = new List<SearchPluginsResponse>(){new SearchPluginsResponse()};
        var totalCount = items.Count();

        return new PagedList<SearchPluginsResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
