using Elsa.Common.Multitenancy;
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Core.Identity.Users.Abstractions;

namespace FSH.Starter.ElsaWorkflow.Infrastructure.Tenants;


public class FinbuckleTenantResolver : Elsa.Common.Multitenancy.ITenantResolver
{
    private readonly IMultiTenantContextAccessor _multiTenantContextAccessor;
    private readonly ITenantService _fshtenantService;

    public FinbuckleTenantResolver(IMultiTenantContextAccessor multiTenantContextAccessor, ITenantService fshtenantService)
    {
        _multiTenantContextAccessor = multiTenantContextAccessor;
        _fshtenantService = fshtenantService;
    }

    public async Task<string?> ResolveAsync(TenantResolverContext context, CancellationToken cancellationToken = default, ICurrentUser currentUser = null)
    {
        // Pull the tenant ID directly from Finbuckle's current context
        // var tenantId = _multiTenantContextAccessor.MultiTenantContext?.TenantInfo?.Id;
        // var tenantId = await _fshtenantService.GetAsync();
        return currentUser.GetTenant();
    }

    public Task<TenantResolverResult> ResolveAsync(TenantResolverContext context)
    {
        var tenantId = _multiTenantContextAccessor.MultiTenantContext?.TenantInfo?.Id;

        return Task.FromResult(TenantResolverResult.Resolved(tenantId));
    }
}
