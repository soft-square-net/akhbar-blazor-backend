using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Domain;
using FSH.Starter.WebApi.Document.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Document.Application.StorageAccounts.Delete.v1;
public sealed class DeleteStorageAccountHandler(
    ILogger<DeleteStorageAccountHandler> logger,
    [FromKeyedServices("document:storage-accounts")] IRepository<StorageAccount> repository)
    : IRequestHandler<DeleteStorageAccountCommand>
{
    public async Task Handle(DeleteStorageAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var storageAccount = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = storageAccount ?? throw new StorageAccountNotFoundException(request.Id);
        await repository.DeleteAsync(storageAccount, cancellationToken);
        logger.LogInformation("Storage Account with id : {StorageAccountId} deleted", storageAccount.Id);
    }
}
