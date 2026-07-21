using System.ComponentModel;
using FSH.Starter.WebApi.Document.Domain;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Application.AccessRules.Get.v1;
public sealed record AccessRuleResponse(
    Guid? Id,
    StorageAccount? StorageAccount,
    string? ResourceOwnerId,
    ResourceOwnerType? ResourceOwnerType,
    bool IsEnabled,
    bool Read,
    bool Write,
    bool Execute,
    Bucket? Bucket,
    string RootPath,
    string Description);
