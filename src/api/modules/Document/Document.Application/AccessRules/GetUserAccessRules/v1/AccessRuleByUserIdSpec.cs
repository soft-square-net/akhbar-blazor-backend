using Ardalis.Specification;
using FSH.Starter.WebApi.Document.Domain;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.AccessRules.GetUserAccessRules.v1;

internal class AccessRuleByUserIdSpec : Specification<AccessRule>
{
    public AccessRuleByUserIdSpec(Guid userId, string[] groupIds)
    {
        Query.Include(x => x.Bucket).Include(x => x.StorageAccount);
        Query.Where(x => (x.ResourceOwnerType == ResourceOwnerType.User && x.ResourceOwnerId == userId.ToString())
            || (groupIds.Length > 0 ? (x.ResourceOwnerType == ResourceOwnerType.Group && groupIds.Contains(x.ResourceOwnerId)) : false)
        );
    }
}
