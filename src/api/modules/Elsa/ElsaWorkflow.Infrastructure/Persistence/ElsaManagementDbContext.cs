// Management Db Context
using Elsa.Persistence.EFCore.Modules.Management;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Constants;

namespace FSH.Starter.ElsaWorkflow.Infrastructure.Persistence;

public class ElsaManagementDbContext : ManagementElsaDbContext   //FshDbContext
{
    public ElsaManagementDbContext(DbContextOptions<ManagementElsaDbContext> options, IServiceProvider serviceProvider) : base(options, serviceProvider)
    {
    }

    //public ElsaManagementDbContext(IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor, DbContextOptions options, IPublisher publisher, IOptions<DatabaseOptions> settings) : base(multiTenantContextAccessor, options, publisher, settings)
    //{
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ElsaManagementDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.ElsaManagment);
    }
}
