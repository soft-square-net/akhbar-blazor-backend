using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Application.StorageAccounts.Get.v1;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Document.Application.StorageAccounts.Search.v1;
public sealed class SearchStorageAccountsHandler(
    [FromKeyedServices("document:storage-accounts")] IReadRepository<StorageAccount> repository)
    : IRequestHandler<SearchStorageAccountsCommand, PagedList<StorageAccountResponse>>
{
    public async Task<PagedList<StorageAccountResponse>> Handle(SearchStorageAccountsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchStorageAccountSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<StorageAccountResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
