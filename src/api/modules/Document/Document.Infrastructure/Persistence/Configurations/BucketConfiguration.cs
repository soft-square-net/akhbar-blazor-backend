
using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Document.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Document.Infrastructure.Persistence.Configurations;
internal class BucketConfiguration : IEntityTypeConfiguration<Bucket>
{
    public void Configure(EntityTypeBuilder<Bucket> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Region).HasMaxLength(100);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.ResourceName).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(250);

        builder.Navigation(b => b.Folders).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.HasOne(m => m.StorageAccount)
            .WithMany(t => t.Buckets)
            .HasForeignKey(m => m.StorageAccountId)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasMany(m => m.Folders)
            .WithOne(t => t.Bucket)
            .HasForeignKey(m => m.BucketId)
            .OnDelete(DeleteBehavior.NoAction);
        // builder.Navigation(b => b.Folders).AutoInclude();
    }
}
