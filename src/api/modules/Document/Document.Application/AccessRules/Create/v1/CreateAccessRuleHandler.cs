using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Domain;
using FSH.Starter.WebApi.Document.Application.Documents.Search.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Document.Appication.AccessRules.Create.v1;

public sealed class CreateAccessRuleHandler(
    ILogger<CreateAccessRuleHandler> logger,
    [FromKeyedServices("document:access-rules")] IRepository<AccessRule> repository,
    [FromKeyedServices("document:storage-accounts")] IReadRepository<StorageAccount> saRepository,
    [FromKeyedServices("document:buckets")] IReadRepository<Bucket> bRepository)
    : IRequestHandler<CreateAccessRuleCommand, CreateAccessRuleResponse>
{
    public async Task<CreateAccessRuleResponse> Handle(CreateAccessRuleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var storageAccount = await saRepository.GetByIdAsync(request.StorageAccountId, cancellationToken);
        if (storageAccount is null)
            ArgumentNullException.ThrowIfNull(request, nameof(request.StorageAccountId));
        var bucket = await bRepository.GetByIdAsync(request.BucketId, cancellationToken);
        if (bucket is null)
            ArgumentNullException.ThrowIfNull(request, nameof(request.BucketId));
        var accessRule = AccessRule.Create(storageAccount, request.ResourceOwnerType, request.ResourceOwnerId, bucket, request.RootPath, request.IsEnabled, request.Read, request.Write, request.Execute, request.Description);
        await repository.AddAsync(accessRule, cancellationToken);
        logger.LogInformation("Access Rule created {AccessRuleId}", accessRule.Id);
        return new CreateAccessRuleResponse(accessRule.Id);
    }
}
