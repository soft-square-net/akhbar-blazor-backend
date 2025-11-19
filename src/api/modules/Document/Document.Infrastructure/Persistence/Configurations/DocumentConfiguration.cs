
using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Document.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Document.Infrastructure.Persistence.Configurations;
internal class DocumentConfiguration : IEntityTypeConfiguration<Domain.Document>
{
    public void Configure(EntityTypeBuilder<Domain.Document> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(1000);
    }
}
