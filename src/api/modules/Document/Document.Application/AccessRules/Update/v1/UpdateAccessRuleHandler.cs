using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Domain;
using FSH.Starter.WebApi.Document.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Document.Application.AccessRules.Update.v1;
public sealed class UpdateAccessRuleHandler(
    ILogger<UpdateAccessRuleHandler> logger,
    [FromKeyedServices("document:access-rules")] IRepository<AccessRule> repository)
    : IRequestHandler<UpdateAccessRuleCommand, UpdateAccessRuleResponse>
{
    public async Task<UpdateAccessRuleResponse> Handle(UpdateAccessRuleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var accessRule = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = accessRule ?? throw new AccessRuleNotFoundException(request.Id);
        var updatedAccessRule = accessRule.Update(request.RootPath, request.IsEnabled, request.Read, request.Write, request.Execute, request.Description);
        await repository.UpdateAsync(updatedAccessRule, cancellationToken);
        logger.LogInformation("AccessRule with id : {AccessRuleId} updated.", accessRule.Id);
        return new UpdateAccessRuleResponse(accessRule.Id);
    }
}
