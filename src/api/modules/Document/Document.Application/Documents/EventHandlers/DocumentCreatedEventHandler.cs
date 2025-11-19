using FSH.Starter.WebApi.Document.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Document.Application.Documents.EventHandlers;

public class DocumentCreatedEventHandler(ILogger<DocumentCreatedEventHandler> logger) : INotificationHandler<DocumentCreated>
{
    public async Task Handle(DocumentCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling document created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling document created domain event..");
    }
}
