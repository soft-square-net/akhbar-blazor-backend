using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.WebApi.Document.Domain;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.AccessRules.GetUserAccessRules.v1;

public sealed record GetUserAccessRulesResponse(
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
