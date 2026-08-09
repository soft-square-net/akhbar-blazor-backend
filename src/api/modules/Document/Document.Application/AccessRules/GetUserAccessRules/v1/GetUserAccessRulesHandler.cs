using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Framework.Core.Identity.Roles;
using FSH.Framework.Core.Identity.Users.Abstractions;
using FSH.Framework.Core.Identity.Users.Dtos;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Application.AccessRules.Get.v1;
using FSH.Starter.WebApi.Document.Domain;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Document.Appication.AccessRules.GetUserAccessRules.v1;

public sealed class GetUserAccessRulesHandler(
    ICurrentUser currentuser, 
    // IRoleService roleService, 
    IUserService userService,
    [FromKeyedServices("document:access-rules")] IReadRepository<AccessRule> repository
) : IRequestHandler<GetUserAccessRulesRequest, List<GetUserAccessRulesResponse>>
{

    public async Task<List<GetUserAccessRulesResponse>> Handle(GetUserAccessRulesRequest request, CancellationToken cancellationToken)
    {
        // Implementation for handling the request and returning the list of access rules
        var UserId = request.UserId ?? currentuser.GetUserId();
        // var User = await userService.GetAsync(UserId.ToString(), cancellationToken);
        List<UserRoleDetail> userRoles = await userService.GetUserRolesAsync(UserId.ToString(), cancellationToken);
        // var groupIds = await roleService.GetRolesAsync(UserId, cancellationToken);
        var accessRules = await repository.ListAsync(new AccessRuleByUserIdSpec(UserId, userRoles.Select(x => x.RoleId.ToString()).ToArray()), cancellationToken);

        // return accessRules.Adapt<List<GetUserAccessRulesResponse>>();
        List<GetUserAccessRulesResponse> results = new();
        foreach (var accessRule in accessRules)
        {
            results.Add(
                    new GetUserAccessRulesResponse(accessRule.Id, accessRule.StorageAccount, accessRule.ResourceOwnerId, accessRule.ResourceOwnerType, accessRule.IsEnabled, accessRule.Read, accessRule.Write, accessRule.Execute, accessRule.Bucket, accessRule.RootPath, accessRule.Description)
                );
        }
        return results;
    }
}
