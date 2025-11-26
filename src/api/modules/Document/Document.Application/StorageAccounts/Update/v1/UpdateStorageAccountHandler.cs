using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Domain;
using FSH.Starter.WebApi.Document.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Document.Application.StorageAccounts.Update.v1;
public sealed class UpdateStorageAccountHandler(
    ILogger<UpdateStorageAccountHandler> logger,
    [FromKeyedServices("document:storage-accounts")] IRepository<StorageAccount> repository)
    : IRequestHandler<UpdateStorageAccountCommand, UpdateStorageAccountResponse>
{
    public async Task<UpdateStorageAccountResponse> Handle(UpdateStorageAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var storageAccount = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = storageAccount ?? throw new StorageAccountNotFoundException(request.Id);
        var updatedStorageAccount = storageAccount.Update(request.AccountName, request.AccessKey, request.SecretKey, request.Description);
        await repository.UpdateAsync(updatedStorageAccount, cancellationToken);
        logger.LogInformation("StorageAccount with id : {StorageAccountId} updated.", storageAccount.Id);
        return new UpdateStorageAccountResponse(storageAccount.Id);
    }
}
