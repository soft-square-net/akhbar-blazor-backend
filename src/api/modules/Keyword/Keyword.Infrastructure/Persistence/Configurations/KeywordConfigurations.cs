using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Keyword.Infrastructure.Persistence.Configurations;

internal class KeywordConfigurations : IEntityTypeConfiguration<Domain.Keyword>
{
    public void Configure(EntityTypeBuilder<Domain.Keyword> builder)
    {
        // KEYWORD CONFIGURATION
        // ==========================================
        builder.IsMultiTenant();
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Value).IsRequired().HasMaxLength(100);

        // Unique index to prevent duplicate keywords
        builder.HasIndex(e => e.Value).IsUnique();

        // Index for fast faceted filtering by Part of Speech or Entity Type
        builder.HasIndex(e => new { e.PartOfSpeech, e.EntityType });
    }
}
