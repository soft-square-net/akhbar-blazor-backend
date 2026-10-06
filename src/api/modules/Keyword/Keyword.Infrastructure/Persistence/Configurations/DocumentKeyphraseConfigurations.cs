using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Keyword.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Keyword.Infrastructure.Persistence.Configurations;

internal class DocumentKeyphraseConfigurations : IEntityTypeConfiguration<DocumentKeyphrase>
{
    public void Configure(EntityTypeBuilder<DocumentKeyphrase> builder)
    {
        // ==========================================
        // DOCUMENT-KEYPHRASE RELATIONSHIP (Many-to-Many)
        // ==========================================
        builder.IsMultiTenant();
        builder.HasKey(dk => new { dk.DocumentId, dk.KeyphraseId });

        builder.HasOne(dk => dk.Document)
            .WithMany(d => d.DocumentKeyphrases)
            .HasForeignKey(dk => dk.DocumentId);

        builder.HasOne(dk => dk.Keyphrase)
            .WithMany(k => k.DocumentKeyphrases)
            .HasForeignKey(dk => dk.KeyphraseId);
    }
}
