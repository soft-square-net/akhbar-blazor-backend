using Microsoft.Extensions.DependencyInjection;
using FSH.Starter.WebApi.Document.Domain.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Caching;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.AccessRules.Get.v1;
public sealed class GetAccessRuleHandler(
    [FromKeyedServices("document:access-rules")] IReadRepository<AccessRule> repository,
    ICacheService cache)
    : IRequestHandler<GetAccessRuleRequest, AccessRuleResponse>
{
    public async Task<AccessRuleResponse> Handle(GetAccessRuleRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"document:{request.Id}",
            async () =>
            {
                var accessRule = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (accessRule == null) throw new AccessRuleNotFoundException(request.Id);
                return new AccessRuleResponse(
                    Id: accessRule.Id,
                    StorageAccount: accessRule.StorageAccount,
                    ResourceOwnerId: accessRule.ResourceOwnerId,
                    ResourceOwnerType: accessRule.ResourceOwnerType,
                    IsEnabled: accessRule.IsEnabled,
                    Read: accessRule.Read,
                    Write: accessRule.Write,
                    Execute: accessRule.Execute,
                    Bucket: accessRule.Bucket,
                    RootPath: accessRule.RootPath,
                    Description: accessRule.Description
                );
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
