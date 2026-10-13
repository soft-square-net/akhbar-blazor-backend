
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Framework.Infrastructure.Tenant;
using FSH.Starter.WebApi.PluginsManager.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Constants;

namespace FSH.Starter.WebApi.PlugindManager.Infrastructure.Persistence;
public sealed class PluginsDbContext : FshDbContext
{
    public PluginsDbContext(IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor, DbContextOptions<PluginsDbContext> options, IPublisher publisher, IOptions<DatabaseOptions> settings)
        : base(multiTenantContextAccessor, options, publisher, settings)
    {
    }
    
    public DbSet<Plugin> Plugins { get; set; } = null!;
    // public DbSet<Domain.File> Files { get; set; } = null!;
    // public DbSet<Folder> Folders { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PluginsDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Plugin);
    }
}
