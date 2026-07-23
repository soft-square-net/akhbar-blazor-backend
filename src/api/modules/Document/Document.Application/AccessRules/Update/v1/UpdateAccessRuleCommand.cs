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
    Guid BucketId,
    [property: DefaultValue(true)] bool IsEnabled,
    [property: DefaultValue(true)] bool Read,
    [property: DefaultValue(false)] bool Write,
    [property: DefaultValue(false)] bool Execute,
    [property: DefaultValue("")] string RootPath,
    [property: DefaultValue("")] string Description
    // Bucket Bucket,
    // StorageAccount StorageAccount
    ) : IRequest<UpdateAccessRuleResponse>;
