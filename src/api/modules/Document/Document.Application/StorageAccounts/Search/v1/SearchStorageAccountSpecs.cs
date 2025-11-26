using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.Document.Application.StorageAccounts.Get.v1;
using FSH.Starter.WebApi.Document.Domain;

namespace FSH.Starter.WebApi.Document.Application.StorageAccounts.Search.v1;
public class SearchStorageAccountSpecs : EntitiesByPaginationFilterSpec<Domain.StorageAccount, StorageAccountResponse>
{
    public SearchStorageAccountSpecs(SearchStorageAccountsCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.AccountName, !command.HasOrderBy())
            .Where(b => b.AccountName.Contains(command.Keyword), !string.IsNullOrEmpty(command.Keyword));
}
