 // Management Db Context
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Constants;

namespace FSH.Starter.ElsaWorkflow.Infrastructure.Persistence;

public class ElsaRuntimeDbContext : RuntimeElsaDbContext  //FshDbContext
{
    public ElsaRuntimeDbContext(DbContextOptions<RuntimeElsaDbContext> options, IServiceProvider serviceProvider) : base(options, serviceProvider)
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ElsaRuntimeDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.ElsaManagment);
    }
}
