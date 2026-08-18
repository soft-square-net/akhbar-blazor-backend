//using MediatR;

//namespace FSH.Starter.Blazor.Infrastructure.Notifications;

//public class StatefulMessageBroker<TNotification> : INotificationHandler<TNotification>
//    where TNotification : INotification
//{
//    // Caches the last triggered event data
//    public TNotification? LatestMessage { get; private set; }

//    // Action that Blazor components can subscribe to safely
//    public event Action<TNotification>? OnMessageReceived;

//    public Task Handle(TNotification notification, CancellationToken cancellationToken)
//    {
//        LatestMessage = notification;

//        // Notify any components that are currently registered
//        OnMessageReceived?.Invoke(notification);

//        return Task.CompletedTask;
//    }
//}

