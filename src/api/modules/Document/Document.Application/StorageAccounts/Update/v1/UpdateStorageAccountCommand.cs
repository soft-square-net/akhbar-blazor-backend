using MediatR;

namespace FSH.Starter.WebApi.Document.Application.StorageAccounts.Update.v1;
public sealed record UpdateStorageAccountCommand(
    Guid Id,
    string? AccountName,
    string? AccessKey,
    string? SecretKey,
    string? Description = null) : IRequest<UpdateStorageAccountResponse>;
