using System.ComponentModel;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Application.AccessRules.Update.v1;
public sealed record UpdateAccessRuleCommand(
    Guid Id,
    Guid StorageAccountId,
    ResourceOwnerType ResourceOwnerType,
    string ResourceOwnerId,
    bool IsEnabled,
    bool Read,
    bool Write,
    bool Execute,
    Guid BucketId,
    string RootPath,
    string Description) : IRequest<UpdateAccessRuleResponse>;
