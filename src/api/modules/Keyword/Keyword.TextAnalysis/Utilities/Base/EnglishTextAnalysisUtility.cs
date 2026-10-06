
using System.Text.RegularExpressions;

namespace FSH.Starter.WebApi.Keyword.TextAnalysis.Utilities.Base;

public class EnglishTextAnalysisUtility : ITextAnalysisUtility
{
    private static readonly Regex PunctuationRegex = new Regex(@"[^\w\s]", RegexOptions.Compiled);
    private readonly HashSet<string> _stopWords;

    public EnglishTextAnalysisUtility(IEnumerable<string>? customStopWords = null)
    {
        var words = customStopWords ?? DefaultEnglishStopWords;
        _stopWords = new HashSet<string>(words.Select(w => w.ToLowerInvariant()));
    }

    public string Normalize(string text)
    {
        return text?.ToLowerInvariant().Trim() ?? string.Empty;
    }

    public IEnumerable<string> Tokenize(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return Enumerable.Empty<string>();

        string normalized = Normalize(text);
        string cleaned = PunctuationRegex.Replace(normalized, " ");
        return cleaned.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
    }

    public IEnumerable<string> RemoveStopWords(IEnumerable<string> tokens)
    {
        return tokens.Where(token => !_stopWords.Contains(token));
    }

    public IDictionary<string, int> GetWordFrequencies(string text, bool filterStopWords = true)
    {
        var tokens = Tokenize(text);
        if (filterStopWords)
        {
            tokens = RemoveStopWords(tokens);
        }

        return tokens.GroupBy(t => t)
                     .ToDictionary(g => g.Key, g => g.Count());
    }

    public IReadOnlySet<string> GetStopWords() => _stopWords;

    private static readonly HashSet<string> DefaultEnglishStopWords = new()
    {
        "a", "an", "the", "and", "or", "but", "if", "because", "as", "until", "while",
        "of", "at", "by", "for", "with", "about", "against", "between", "into", "through",
        "during", "before", "after", "above", "below", "to", "from", "up", "down", "in",
        "out", "on", "off", "over", "under", "again", "further", "then", "once", "here",
        "there", "when", "where", "why", "how", "all", "any", "both", "each", "few",
        "more", "most", "other", "some", "such", "no", "nor", "not", "only", "own",
        "same", "so", "than", "too", "very", "s", "t", "can", "will", "just", "don",
        "should", "now", "i", "me", "my", "myself", "we", "our", "ours", "he", "him",
        "his", "she", "her", "it", "its", "they", "them", "their", "what", "which", "who"
    };
}
