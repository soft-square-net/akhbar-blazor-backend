using FSH.Starter.WebApi.Document.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Document.Application.Buckets.EventHandlers;

public class BucketCreatedEventHandler(ILogger<BucketCreatedEventHandler> logger) : INotificationHandler<BucketCreated>
{
    public async Task Handle(BucketCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling Bucket created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling Bucket created domain event..");
    }
}
