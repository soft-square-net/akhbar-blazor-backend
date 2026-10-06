using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Keyword.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Keyword.Infrastructure.Persistence.Configurations;

internal class KeyphraseConfigurations : IEntityTypeConfiguration<Keyphrase>
{
    public void Configure(EntityTypeBuilder<Keyphrase> builder)
    {
        // ==========================================
        // KEYPHRASE CONFIGURATION
        // ==========================================
        builder.IsMultiTenant();
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Value).IsRequired().HasMaxLength(255);
        builder.HasIndex(e => e.Value).IsUnique();
    }
}
