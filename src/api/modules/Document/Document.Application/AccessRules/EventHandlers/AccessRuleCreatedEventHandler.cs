using FSH.Starter.WebApi.Document.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Document.Application.AccessRules.EventHandlers;

public class AccessRuleCreatedEventHandler(ILogger<AccessRuleCreatedEventHandler> logger) : INotificationHandler<AccessRuleCreated>
{
    public async Task Handle(AccessRuleCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling document storage user created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling document storage user created domain event..");
    }
}
