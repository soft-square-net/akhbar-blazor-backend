using FSH.Starter.WebApi.Document.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Document.Application.Documents.EventHandlers;

public class StorageAccountCreatedEventHandler(ILogger<StorageAccountCreatedEventHandler> logger) : INotificationHandler<StorageAccountCreated>
{
    public async Task Handle(StorageAccountCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling document created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling document created domain event..");
    }
}
