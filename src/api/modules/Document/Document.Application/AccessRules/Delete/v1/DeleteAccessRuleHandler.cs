using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Domain;
using FSH.Starter.WebApi.Document.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Document.Application.AccessRules.Delete.v1;
public sealed class DeleteAccessRuleHandler(
    ILogger<DeleteAccessRuleHandler> logger,
    [FromKeyedServices("document:access-rules")] IRepository<AccessRule> repository)
    : IRequestHandler<DeleteAccessRuleCommand>
{
    public async Task Handle(DeleteAccessRuleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var accessRule = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = accessRule ?? throw new AccessRuleNotFoundException(request.Id);
        await repository.DeleteAsync(accessRule, cancellationToken);
        logger.LogInformation("Storage Account with id : {AccessRuleId} deleted", accessRule.Id);
    }
}
