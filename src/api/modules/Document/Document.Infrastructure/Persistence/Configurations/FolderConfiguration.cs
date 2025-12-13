
using System.Reflection.Emit;
using System.Security.Claims;
using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Document.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Document.Infrastructure.Persistence.Configurations;
internal class FolderConfiguration : IEntityTypeConfiguration<Folder>
{
    public void Configure(EntityTypeBuilder<Folder> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(1000);


        builder.HasOne(m => m.Parent)
            .WithMany(t => t.Children)
            .HasForeignKey(m => m.ParentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(m => m.Files)
            .WithOne(t => t.Folder)
            .HasForeignKey(m => m.FolderId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
