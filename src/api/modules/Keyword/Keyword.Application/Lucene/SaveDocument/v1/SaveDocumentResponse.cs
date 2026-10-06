using FSH.Starter.WebApi.Keyword.TextAnalysis.Common.Models.SEO;

namespace FSH.Starter.WebApi.Keyword.Application.Lucene.SaveDocument.v1;

public record SaveDocumentResponse(
    Guid DocumentId,
    SeoAnalysisResult SeoAnalysis,
    GrammarCheckResult GrammarCheck,
    DateTime SavedAtUtc);
