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
    [FromKeyedServices("document:access-rules")] IRepository<AccessRule> repository)
    : IRequestHandler<CreateAccessRuleCommand, CreateAccessRuleResponse>
{
    public async Task<CreateAccessRuleResponse> Handle(CreateAccessRuleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var accessRule = AccessRule.Create(request.StorageAccount, request.ResourceOwnerType, request.ResourceOwnerId, request.Bucket, request.RootPath, request.IsEnabled, request.Read, request.Write, request.Execute, request.Description);
        await repository.AddAsync(accessRule, cancellationToken);
        logger.LogInformation("Access Rule created {AccessRuleId}", accessRule.Id);
        return new CreateAccessRuleResponse(accessRule.Id);
    }
}
