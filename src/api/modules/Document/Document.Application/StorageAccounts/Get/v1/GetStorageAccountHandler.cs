using Microsoft.Extensions.DependencyInjection;
using FSH.Starter.WebApi.Document.Domain.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Caching;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.StorageAccounts.Get.v1;
public sealed class GetStorageAccountHandler(
    [FromKeyedServices("document:storage-accounts")] IReadRepository<StorageAccount> repository,
    ICacheService cache)
    : IRequestHandler<GetStorageAccountRequest, StorageAccountResponse>
{
    public async Task<StorageAccountResponse> Handle(GetStorageAccountRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"document:{request.Id}",
            async () =>
            {
                var storageAccount = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (storageAccount == null) throw new StorageAccountNotFoundException(request.Id);
                return new StorageAccountResponse(storageAccount.Id, storageAccount.AccountName, storageAccount.Description);
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
