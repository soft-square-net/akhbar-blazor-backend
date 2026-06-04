
using System.ComponentModel;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.AccessRules.Create.v1;

public sealed record CreateAccessRuleCommand(
    StorageAccount StorageAccount,
    Guid ResourceOwnerId,
    ResourceOwnerType ResourceOwnerType,
    [property: DefaultValue(true)] bool IsEnabled,
    [property: DefaultValue(true)] bool Read,
    [property: DefaultValue(false)] bool Write,
    [property: DefaultValue(false)] bool Execute,
    Bucket Bucket,
    string RootPath,
    [property: DefaultValue("Descriptive Description")] string Description
) : IRequest<CreateAccessRuleResponse>;
