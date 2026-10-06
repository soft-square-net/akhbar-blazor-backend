
using System.Text.RegularExpressions;
using global::FSH.Starter.WebApi.Keyword.TextAnalysis.Common.Models.SEO;
using global::FSH.Starter.WebApi.Keyword.TextAnalysis.Utilities.Base;
using Shared.Enums;

namespace FSH.Starter.WebApi.Keyword.TextAnalysis.Services;


public class SeoAnalysisService : ISeoAnalysisService
{
    public SeoAnalysisResult AnalyzeDocument(
        string content,
        string title,
        string? focusKeyword = null,
        string? metaDescription = null,
        Language language = Language.English)
    {
        var issues = new List<SeoIssue>();
        int score = 100;

        // Resolve language-specific analyzer
        var analyzer = TextAnalysisUtilityFactory.Create(language);
        string sanitizedContent = analyzer.Normalize(content);
        var tokens = analyzer.Tokenize(content).ToList();
        int wordCount = tokens.Count;

        // 1. Content Length Check
        if (wordCount < 300)
        {
            score -= 15;
            issues.Add(new SeoIssue("LEN_LOW", "Content is too short.", SeoIssueSeverity.Warning, "Aim for at least 300-600 words for better search ranking."));
        }

        // 2. Title Length Check (Optimal: 50-60 chars)
        if (string.IsNullOrWhiteSpace(title) || title.Length < 30 || title.Length > 60)
        {
            score -= 10;
            issues.Add(new SeoIssue("TITLE_LEN", "Title tag length is sub-optimal.", SeoIssueSeverity.Warning, "Keep page titles between 30 and 60 characters."));
        }

        // 3. Meta Description Check (Optimal: 120-160 chars)
        if (string.IsNullOrWhiteSpace(metaDescription) || metaDescription.Length < 120 || metaDescription.Length > 160)
        {
            score -= 10;
            issues.Add(new SeoIssue("META_LEN", "Meta description length is sub-optimal.", SeoIssueSeverity.Warning, "Keep meta descriptions between 120 and 160 characters."));
        }

        // 4. Focus Keyword Checks
        double keywordDensity = 0;
        bool foundInTitle = false;
        bool foundInFirstParagraph = false;
        bool foundInHeadings = false;

        if (!string.IsNullOrWhiteSpace(focusKeyword))
        {
            string cleanKeyword = analyzer.Normalize(focusKeyword);

            // Title Check
            foundInTitle = analyzer.Normalize(title).Contains(cleanKeyword);
            if (!foundInTitle)
            {
                score -= 15;
                issues.Add(new SeoIssue("KW_TITLE", "Focus keyword missing from Title.", SeoIssueSeverity.Error, "Add your target keyword near the beginning of the title."));
            }

            // First Paragraph Check
            string firstParagraph = content.Split(new[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "";
            foundInFirstParagraph = analyzer.Normalize(firstParagraph).Contains(cleanKeyword);
            if (!foundInFirstParagraph)
            {
                score -= 10;
                issues.Add(new SeoIssue("KW_FIRST_PARA", "Focus keyword missing from opening paragraph.", SeoIssueSeverity.Warning, "Include your focus keyword in the first 100 words."));
            }

            // Keyword Density (Target: 1.0% - 2.5%)
            int keywordOccurrences = Regex.Matches(sanitizedContent, Regex.Escape(cleanKeyword)).Count;
            keywordDensity = wordCount > 0 ? (double)keywordOccurrences / wordCount * 100 : 0;

            if (keywordDensity < 0.5)
            {
                score -= 10;
                issues.Add(new SeoIssue("KW_DENSITY_LOW", $"Keyword density is too low ({keywordDensity:F1}%).", SeoIssueSeverity.Warning, "Increase keyword usage slightly to around 1% to 2.5%."));
            }
            else if (keywordDensity > 3.0)
            {
                score -= 20;
                issues.Add(new SeoIssue("KW_STUFFING", $"Keyword density is too high ({keywordDensity:F1}%).", SeoIssueSeverity.Error, "Avoid keyword stuffing. Keep density below 2.5%."));
            }
        }

        // 5. Readability Score Calculation (Flesch Reading Ease for English)
        double readabilityScore = CalculateReadabilityScore(content, language, wordCount);
        if (readabilityScore < 50)
        {
            score -= 10;
            issues.Add(new SeoIssue("READABILITY_LOW", "Content may be difficult to read.", SeoIssueSeverity.Info, "Use shorter sentences and simpler vocabulary to improve readability."));
        }

        return new SeoAnalysisResult(
            Math.Max(0, score),
            wordCount,
            Math.Round(keywordDensity, 2),
            Math.Round(readabilityScore, 1),
            foundInTitle,
            foundInFirstParagraph,
            foundInHeadings,
            issues
        );
    }

    private static double CalculateReadabilityScore(string content, Language language, int wordCount)
    {
        if (wordCount == 0) return 0;

        int sentences = Regex.Matches(content, @"[.!?]+").Count;
        if (sentences == 0) sentences = 1;

        if (language == Language.English)
        {
            int syllables = CountEnglishSyllables(content);
            // Flesch Reading Ease Formula
            return 206.835 - (1.015 * ((double)wordCount / sentences)) - (84.6 * ((double)syllables / wordCount));
        }

        // Simplified Sentence Complexity index for Arabic / Generic
        double avgWordsPerSentence = (double)wordCount / sentences;
        return Math.Max(0, 100 - (avgWordsPerSentence * 2));
    }

    private static int CountEnglishSyllables(string text)
    {
        var words = text.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int count = 0;
        foreach (var word in words)
        {
            var num = Regex.Matches(word, @"[aeiouy]{1,2}").Count;
            if (word.EndsWith("e")) num--;
            count += Math.Max(1, num);
        }
        return count;
    }
}
