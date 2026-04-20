

using Elsa.Common.Multitenancy;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Infrastructure.Tenant;
using Microsoft.Extensions.Configuration;

namespace FSH.Starter.ElsaWorkflow.Infrastructure.Tenants;


public class FinbuckleTenantProvider : ITenantsProvider
{
    private readonly IMultiTenantStore<FshTenantInfo> _tenantStore;

    public FinbuckleTenantProvider(IMultiTenantStore<FshTenantInfo> tenantStore)
    {
        _tenantStore = tenantStore;
    }

    public async Task<IEnumerable<Tenant>> ListAsync(CancellationToken cancellationToken = default)
    {
        // Fetch all tenants from Finbuckle's store
        var finbuckleTenants = await _tenantStore.GetAllAsync();

        // Map Finbuckle's TenantInfo to Elsa's Tenant entity
        return finbuckleTenants.Select(t => new Tenant
        {
            Id = t.Id,
            Name = t.Name ?? t.Identifier,
            // You can map additional configuration if needed
            Configuration = new ConfigurationBuilder().AddInMemoryCollection(
                new Dictionary<string, string?> { ["Identifier"] = t.Identifier }
            ).Build()
        });
    }

    public async Task<Tenant?> FindAsync(TenantFilter filter, CancellationToken cancellationToken = default)
    {
        // Try to find the tenant by ID or Name from Finbuckle
        var finbuckleTenant = !string.IsNullOrEmpty(filter.Id)
            ? await _tenantStore.TryGetAsync(filter.Id)
            : await _tenantStore.TryGetByIdentifierAsync(filter.Id!);

        if (finbuckleTenant == null) return null;

        return new Tenant
        {
            Id = finbuckleTenant.Id,
            Name = finbuckleTenant.Name ?? finbuckleTenant.Identifier
        };
    }
}
