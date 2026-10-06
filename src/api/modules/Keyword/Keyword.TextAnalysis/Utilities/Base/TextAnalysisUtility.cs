using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FSH.Starter.WebApi.Keyword.TextAnalysis.Common.Models;

namespace FSH.Starter.WebApi.Keyword.TextAnalysis.Utilities.Base;

public static class TextAnalysisUtility
{
    // High-performance immutable set for stop-word lookups (.NET 8/9 feature)
    private static readonly FrozenSet<string> StopWords = new string[]
    {
        "a", "about", "above", "after", "again", "against", "all", "am", "an", "and", "any", "are", "aren't",
        "as", "at", "be", "because", "been", "before", "being", "below", "between", "both", "but", "by",
        "can't", "cannot", "could", "couldn't", "did", "didn't", "do", "does", "doesn't", "doing", "don't",
        "down", "during", "each", "few", "for", "from", "further", "had", "hadn't", "has", "hasn't", "have",
        "haven't", "having", "he", "he'd", "he'll", "he's", "her", "here", "here's", "hers", "herself",
        "him", "himself", "his", "how", "how's", "i", "i'd", "i'll", "i'm", "i've", "if", "in", "into",
        "is", "isn't", "it", "it's", "its", "itself", "let's", "me", "more", "most", "mustn't", "my",
        "myself", "no", "nor", "not", "of", "off", "on", "once", "only", "or", "other", "ought", "our",
        "ours", "ourselves", "out", "over", "own", "same", "shan't", "she", "she'd", "she'll", "she's",
        "should", "shouldn't", "so", "some", "such", "than", "that", "that's", "the", "their", "theirs",
        "them", "themselves", "then", "there", "there's", "these", "they", "they'd", "they'll", "they're",
        "they've", "this", "those", "through", "to", "too", "under", "until", "up", "very", "was", "wasn't",
        "we", "we'd", "we'll", "we're", "we've", "were", "weren't", "what", "what's", "when", "when's",
        "where", "where's", "which", "while", "who", "who's", "whom", "why", "why's", "with", "won't",
        "would", "wouldn't", "you", "you'd", "you'll", "you're", "you've", "your", "yours", "yourself", "yourselves"
    }.ToFrozenSet(StringComparer.OrdinalIgnoreCase);

    private static readonly Regex HtmlTagRegex = new(@"<[^>]*>", RegexOptions.Compiled);

    private static readonly Regex PunctuationRegex = new(@"[^\w\s-]", RegexOptions.Compiled);

    private static readonly Regex WhitespaceRegex = new(@"\s+", RegexOptions.Compiled);

    // =========================================================================
    // 1. SANITIZATION & NORMALIZATION
    // =========================================================================

    /// <summary>
    /// Strips HTML tags, punctuation, special symbols, and extra spaces. Converts to lowercase.
    /// </summary>
    public static string Sanitize(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        // Remove HTML tags
        string clean = HtmlTagRegex.Replace(input, " ");

        // Remove punctuation (except inner hyphens)
        clean = PunctuationRegex.Replace(clean, " ");

        // Normalize whitespaces
        clean = WhitespaceRegex.Replace(clean, " ").Trim().ToLowerInvariant();

        return clean;
    }

    /// <summary>
    /// Checks whether a token is a stop word or invalid length.
    /// </summary>
    public static bool IsValidKeyword(string word, int minLength = 2, int maxLength = 50)
    {
        if (string.IsNullOrWhiteSpace(word)) return false;
        if (word.Length < minLength || word.Length > maxLength) return false;

        return !StopWords.Contains(word);
    }

    // =========================================================================
    // 2. TOKENIZATION & EXTRACTION
    // =========================================================================

    /// <summary>
    /// Tokenizes content into sanitized, non-stop-word keywords.
    /// </summary>
    public static List<string> ExtractTokens(string text, bool filterStopWords = true)
    {
        string sanitized = Sanitize(text);
        if (string.IsNullOrEmpty(sanitized)) return new List<string>();

        string[] words = sanitized.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return words
            .Where(w => !filterStopWords || IsValidKeyword(w))
            .ToList();
    }

    /// <summary>
    /// Extracts N-gram Keyphrases (e.g., bi-grams or tri-grams) and their frequencies.
    /// </summary>
    public static Dictionary<string, int> ExtractKeyphrases(string text, int gramSize = 2)
    {
        var frequencies = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var tokens = ExtractTokens(text, filterStopWords: false); // Keep stopwords context for phrases

        if (tokens.Count < gramSize) return frequencies;

        for (int i = 0; i <= tokens.Count - gramSize; i++)
        {
            var gramWords = tokens.Skip(i).Take(gramSize).ToList();

            // Ignore phrase if it starts or ends with a stop word
            if (StopWords.Contains(gramWords.First()) || StopWords.Contains(gramWords.Last()))
                continue;

            string phrase = string.Join(" ", gramWords);

            if (phrase.Length >= 4)
            {
                frequencies[phrase] = frequencies.GetValueOrDefault(phrase, 0) + 1;
            }
        }

        return frequencies;
    }

    // =========================================================================
    // 3. RELEVANCE & TF-IDF SCORE CALCULATIONS
    // =========================================================================

    /// <summary>
    /// Calculates Term Frequency (TF): Term Count / Total Document Words.
    /// </summary>
    public static float CalculateTermFrequency(int termCount, int totalDocumentWords)
    {
        if (totalDocumentWords == 0) return 0f;
        return (float)termCount / totalDocumentWords;
    }

    /// <summary>
    /// Calculates Smooth Inverse Document Frequency (IDF): log(1 + (Total Docs / Docs With Term)) + 1.
    /// </summary>
    public static float CalculateIdf(int totalDocuments, int documentsWithTerm)
    {
        if (totalDocuments <= 0) return 1.0f;

        // Smoothed IDF formula (similar to scikit-learn)
        return (float)(Math.Log((1.0 + totalDocuments) / (1.0 + documentsWithTerm)) + 1.0);
    }

    /// <summary>
    /// Calculates the final TF-IDF Relevance Score.
    /// </summary>
    public static float CalculateTfIdfScore(int termCountInDoc, int totalWordsInDoc, int totalDocuments, int documentsWithTerm)
    {
        float tf = CalculateTermFrequency(termCountInDoc, totalWordsInDoc);
        float idf = CalculateIdf(totalDocuments, documentsWithTerm);
        return tf * idf;
    }

    // =========================================================================
    // 4. FULL DOCUMENT ANALYSIS PIPELINE
    // =========================================================================

    /// <summary>
    /// Analyzes raw text and returns complete statistical metrics including keyword/keyphrase counts.
    /// </summary>
    public static DocumentAnalysisMetrics AnalyzeContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return new DocumentAnalysisMetrics(0, 0, 0, 0, new(), new());
        }

        var allTokens = ExtractTokens(content, filterStopWords: false);
        var validKeywords = allTokens.Where(word => IsValidKeyword(word)).ToList();

        // 1. Keyword Frequencies
        var keywordFrequencies = validKeywords
            .GroupBy(k => k, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.Count());

        // 2. Bi-gram Keyphrase Frequencies
        var keyphraseFrequencies = ExtractKeyphrases(content, gramSize: 2);

        // 3. Document Metrics
        int totalWords = allTokens.Count;
        int uniqueWords = keywordFrequencies.Count;
        int charCount = content.Length;
        int estReadingTime = (int)Math.Ceiling(totalWords / 200.0); // Avg reading speed: 200 wpm

        return new DocumentAnalysisMetrics(
            totalWords,
            uniqueWords,
            charCount,
            estReadingTime,
            keywordFrequencies,
            keyphraseFrequencies
        );
    }
}
