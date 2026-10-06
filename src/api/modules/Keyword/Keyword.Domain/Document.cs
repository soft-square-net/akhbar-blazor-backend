using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

// ! using NpgsqlTypes; // Npgsql package for Postgres support
namespace FSH.Starter.WebApi.Keyword.Domain;

public class Document : AuditableEntity, IAggregateRoot
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; } // The raw text

    // Full-Text Search Vector for PostgreSQL (Ignored by MSSQL provider mapping)
    // ! public NpgsqlTsVector SearchVector { get; set; } = null!;

    // Navigation properties
    public ICollection<DocumentKeyword> DocumentKeywords { get; set; } = new List<DocumentKeyword>();
    public ICollection<DocumentKeyphrase> DocumentKeyphrases { get; set; } = new List<DocumentKeyphrase>();


}
