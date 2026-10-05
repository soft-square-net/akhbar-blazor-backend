
using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Keyword.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Keyword.Infrastructure.Persistence.Configurations;

internal class DocumentKeywordConfigurations : IEntityTypeConfiguration<DocumentKeyword>
{
    public void Configure(EntityTypeBuilder<DocumentKeyword> builder)
    {
        // ==========================================
        // DOCUMENT-KEYWORD RELATIONSHIP (Many-to-Many)
        // ==========================================
        builder.IsMultiTenant();
            // Composite Primary Key
            builder.HasKey(dk => new { dk.DocumentId, dk.KeywordId });

            builder.HasOne(dk => dk.Document)
                  .WithMany(d => d.DocumentKeywords)
                  .HasForeignKey(dk => dk.DocumentId);

            builder.HasOne(dk => dk.Keyword)
                  .WithMany(k => k.DocumentKeywords)
                  .HasForeignKey(dk => dk.KeywordId);

            // Index for sorting search results by relevance
            builder.HasIndex(dk => dk.RelevanceScore);

    }
}
