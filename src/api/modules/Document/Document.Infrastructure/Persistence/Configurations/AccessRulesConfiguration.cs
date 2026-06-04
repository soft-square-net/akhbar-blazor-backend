
using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Document.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Document.Infrastructure.Persistence.Configurations;
internal class AccessRuleConfiguration : IEntityTypeConfiguration<AccessRule>
{
    public void Configure(EntityTypeBuilder<AccessRule> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ResourceOwnerId).HasMaxLength(100);
    }
}
