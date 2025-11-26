
using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Document.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Document.Infrastructure.Persistence.Configurations;
internal class StorageAccountConfiguration : IEntityTypeConfiguration<StorageAccount>
{
    public void Configure(EntityTypeBuilder<StorageAccount> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.AccountName).HasMaxLength(100);
        builder.Property(x => x.AccessKey).HasMaxLength(20);
        builder.Property(x => x.SecretKey).HasMaxLength(40);
        builder.Property(x => x.Description).HasMaxLength(1000);
    }
}
