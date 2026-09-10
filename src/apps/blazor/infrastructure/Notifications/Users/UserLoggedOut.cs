using System.Security.Claims;
using FSH.Starter.Blazor.Shared.Notifications;

namespace FSH.Starter.Blazor.Infrastructure.Notifications.Users;

public record UserLoggedOut(ClaimsPrincipal UserInfo, string? Message) : INotificationMessage;

