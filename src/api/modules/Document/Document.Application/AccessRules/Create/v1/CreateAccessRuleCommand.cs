
using System.ComponentModel;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.AccessRules.Create.v1;

public sealed record CreateAccessRuleCommand(

    //Bucket Bucket,
    //StorageAccount StorageAccount,
    Guid BucketId,
    Guid StorageAccountId,
    ResourceOwnerType ResourceOwnerType,
    string ResourceOwnerId,
    string RootPath,
    [property: DefaultValue(true)] bool IsEnabled,
    [property: DefaultValue(true)] bool Read,
    [property: DefaultValue(false)] bool Write,
    [property: DefaultValue(false)] bool Execute,
    [property: DefaultValue("Descriptive Description")] string Description
) : IRequest<CreateAccessRuleResponse>;
