using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Keyword.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NpgsqlTypes;

namespace FSH.Starter.WebApi.Keyword.Infrastructure.Persistence.Configurations;

internal class DocumentConfigurations : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Title).HasMaxLength(256).IsRequired();
        // ----------------------------------------------------
        // POSTGRESQL FULL-TEXT SEARCH CONFIGURATION
        // ----------------------------------------------------
        // Get the core relational provider annotation
        var providerAnnotation = builder.Metadata.Model.FindAnnotation("ProviderName");

        // Extract the provider name string safely
        string providerName = providerAnnotation?.Value?.ToString();

        // Check if the current context run is MSSQL/Npgsql
        bool isSqlServer = providerName == "Microsoft.EntityFrameworkCore.SqlServer";
        bool isNpgsql = providerName == "Microsoft.EntityFrameworkCore.PostgreSQL";

        if (isNpgsql)
        {
            /*builder.HasGeneratedTsVectorColumn(
                    d => d.SearchVector,
                    "english",
                    d => new { d.Title, d.Content }
                )
                .HasIndex(d => d.SearchVector)
                .HasMethod("gin"); // High-performance GIN index*/

            // Auto-generated tsvector combining Title (Weight A) and Content (Weight B)
            builder.Property<NpgsqlTsVector>("SearchVector");

            // PostgreSQL Shadow Property + Generated Vector Index
            builder.HasGeneratedTsVectorColumn(
                    d => EF.Property<NpgsqlTsVector>(d, "SearchVector"),
                    "english",
                    d => new { d.Title, d.Content }
                )
                .HasIndex("SearchVector")
                .HasMethod("gin");
        }

        // ----------------------------------------------------
        // MSSQL FULL-TEXT SEARCH CONFIGURATION
        // Note: MSSQL Full-Text Catalogs are created via SQL Migrations
        // ----------------------------------------------------
        if (isSqlServer)
        {
            // Add composite index for standard fallbacks & filtering
            builder.HasIndex(d => new { d.Title });
        }
    }
}
