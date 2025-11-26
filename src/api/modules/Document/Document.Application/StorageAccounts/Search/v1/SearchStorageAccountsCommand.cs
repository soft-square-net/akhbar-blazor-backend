using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.Document.Application.StorageAccounts.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.StorageAccounts.Search.v1;

public class SearchStorageAccountsCommand : PaginationFilter, IRequest<PagedList<StorageAccountResponse>>
{
    //public string? AccountName { get; set; }
    //public string? Description { get; set; }
}
