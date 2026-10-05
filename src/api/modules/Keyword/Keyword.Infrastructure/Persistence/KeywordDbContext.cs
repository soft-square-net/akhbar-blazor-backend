
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Framework.Infrastructure.Tenant;
using FSH.Starter.WebApi.Keyword.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Constants;

namespace FSH.Starter.WebApi.Keyword.Infrastructure.Persistence;

//public class TextAnalysisDbContext : DbContext
public class KeywordDbContext : FshDbContext
{
    public KeywordDbContext(IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor, DbContextOptions<KeywordDbContext> options, IPublisher publisher, IOptions<DatabaseOptions> settings)
        : base(multiTenantContextAccessor, options, publisher, settings)
    {
    }

    public DbSet<Domain.Keyword> Keywords { get; set; }
    public DbSet<Keyphrase> Keyphrases { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<DocumentKeyword> DocumentKeywords { get; set; }
    public DbSet<DocumentKeyphrase> DocumentKeyphrases { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(KeywordDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Keyword);
    }
}
