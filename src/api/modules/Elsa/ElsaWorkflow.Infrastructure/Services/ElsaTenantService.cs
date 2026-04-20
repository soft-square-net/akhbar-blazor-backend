
using Elsa.Common.Multitenancy;
using Mapster;

namespace ElsaWorkflow.Infrastructure.Services;

internal class ElsaTenantService : ITenantService
{
    private readonly FSH.Framework.Core.Tenant.Abstractions.ITenantService _service;
    public ElsaTenantService(FSH.Framework.Core.Tenant.Abstractions.ITenantService tenantService)
    {
        _service = tenantService;
    }
    public async Task ActivateTenantsAsync(CancellationToken cancellationToken = default)
    {
        await _service.ActivateAsync("", cancellationToken);
    }

    public async Task DeactivateTenantsAsync(CancellationToken cancellationToken = default)
    {
        await _service.DeactivateAsync("");
    }

    public async Task<Tenant?> FindAsync(string id, CancellationToken cancellationToken = default)
    {
        return (await _service.GetByIdAsync(id)).Adapt<Tenant?>();
    }

    public async Task<Tenant?> FindAsync(TenantFilter filter, CancellationToken cancellationToken = default)
    {
        foreach (var tenant in (await _service.GetAllAsync()))
        {
            if (filter.Id != null && tenant.Id != filter.Id)
                continue;
            return tenant.Adapt<Tenant?>();
        }
        return null;
    }

    public async Task<Tenant> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return (await _service.GetByIdAsync(id)).Adapt<Tenant?>() ?? throw new KeyNotFoundException($"Tenant with id '{id}' not found.");
    }

    public async Task<Tenant> GetAsync(TenantFilter filter, CancellationToken cancellationToken = default)
    {
        return (await FindAsync(filter, cancellationToken)) ?? throw new KeyNotFoundException("Tenant not found.");
    }

    public async Task<IEnumerable<Tenant>> ListAsync(CancellationToken cancellationToken = default)
    {
        return (await _service.GetAllAsync()).Adapt<List<Tenant>>();
    }

    public async Task<IEnumerable<Tenant>> ListAsync(TenantFilter filter, CancellationToken cancellationToken = default)
    {
        var tenants = new List<Tenant>();
        foreach (var tenant in (await ListAsync(cancellationToken)))
        {
            if (filter.Id != null && tenant.Id != filter.Id)
                continue;
            tenants.Add(tenant.Adapt<Tenant>());
        }
        return tenants;
    }

    public async Task RefreshAsync(CancellationToken cancellationToken = default)
    {
        await ListAsync(cancellationToken);
    }
}
