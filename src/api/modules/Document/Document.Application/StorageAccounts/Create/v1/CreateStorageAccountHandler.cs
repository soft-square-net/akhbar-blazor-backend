using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Domain;
using FSH.Starter.WebApi.Document.Application.Documents.Search.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Document.Appication.StorageAccounts.Create.v1;

public sealed class CreateStorageAccountHandler(
    ILogger<CreateStorageAccountHandler> logger,
    [FromKeyedServices("document:storage-accounts")] IRepository<StorageAccount> repository)
    : IRequestHandler<CreateStorageAccountCommand, CreateStorageAccountResponse>
{
    public async Task<CreateStorageAccountResponse> Handle(CreateStorageAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var storageAccount = StorageAccount.Create(request.StorageProvider, request.AccountName, request.AccessKey, request.SecretKey, request.Description);
        await repository.AddAsync(storageAccount, cancellationToken);
        logger.LogInformation("Storage Account created {StorageAccountId}", storageAccount.Id);
        return new CreateStorageAccountResponse(storageAccount.Id);
    }
}
