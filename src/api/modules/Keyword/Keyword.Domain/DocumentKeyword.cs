
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace FSH.Starter.WebApi.Keyword.Domain;

public class DocumentKeyword: AuditableEntity<int>, IAggregateRoot
{
    public int DocumentId { get; set; }
    public Document Document { get; set; }

    public int KeywordId { get; set; }
    public Keyword Keyword { get; set; }

    // Text Analysis Metrics
    public int Frequency { get; set; }     // How many times it appeared
    public float RelevanceScore { get; set; } // e.g., TF-IDF score for search ranking
}
