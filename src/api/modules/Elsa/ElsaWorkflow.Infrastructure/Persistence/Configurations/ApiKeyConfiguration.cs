
using Finbuckle.MultiTenant;
using FSH.Starter.ElsaWorkflow.Infrastructure.Auth.ApiKey;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.ElsaWorkflow.Infrastructure.Persistence.Configurations;
internal class ApiKeyConfiguration : IEntityTypeConfiguration<ApiKey>
{
    public void Configure(EntityTypeBuilder<ApiKey> builder)
    {
        builder.IsMultiTenant();
        // builder.HasKey(x => x.Id);
        builder.HasNoKey();
    }
}
