
using Shared.Enums;

namespace FSH.Starter.WebApi.Keyword.TextAnalysis.Common.Models.SEO;

public enum SeoIssueSeverity
{
    Info,
    Warning,
    Error
}

public record SeoIssue(string Code, string Message, SeoIssueSeverity Severity, string Recommendation);

public record SeoAnalysisResult(
    int OverallScore, // 0 - 100
    int WordCount,
    double KeywordDensity, // Percentage (e.g., 1.8%)
    double ReadabilityScore, // Flesch Reading Ease or equivalent
    bool FoundInTitle,
    bool FoundInFirstParagraph,
    bool FoundInHeadings,
    List<SeoIssue> Issues
);

public interface ISeoAnalysisService
{
    SeoAnalysisResult AnalyzeDocument(
        string content,
        string title,
        string? focusKeyword = null,
        string? metaDescription = null,
        Language language = Language.English);
}
