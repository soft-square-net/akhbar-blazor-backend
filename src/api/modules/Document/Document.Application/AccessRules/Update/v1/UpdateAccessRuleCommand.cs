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
    [property: DefaultValue(true)] bool IsEnabled,
    [property: DefaultValue(true)] bool Read,
    bool Write,
    bool Execute,
    Guid BucketId,
    string RootPath,
    string Description
    // Bucket Bucket,
    // StorageAccount StorageAccount
    ) : IRequest<UpdateAccessRuleResponse>;
