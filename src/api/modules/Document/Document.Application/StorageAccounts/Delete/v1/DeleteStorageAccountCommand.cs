using MediatR;

namespace FSH.Starter.WebApi.Document.Application.StorageAccounts.Delete.v1;
public sealed record DeleteStorageAccountCommand(
    Guid Id) : IRequest;
