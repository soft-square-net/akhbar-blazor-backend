
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Document.Infrastructure.Persistence.Configurations;
internal class FileConfiguration : IEntityTypeConfiguration<Domain.File>
{
    public void Configure(EntityTypeBuilder<Domain.File> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(250);
    }
}
