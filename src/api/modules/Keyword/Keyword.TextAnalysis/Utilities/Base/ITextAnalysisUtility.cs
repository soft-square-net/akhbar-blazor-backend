namespace FSH.Starter.WebApi.Keyword.TextAnalysis.Utilities.Base;

public interface ITextAnalysisUtility
{
    /// <summary>
    /// Normalizes characters (e.g., letter variations, removing diacritics/accents).
    /// </summary>
    string Normalize(string text);

    /// <summary>
    /// Splits text into individual word tokens.
    /// </summary>
    IEnumerable<string> Tokenize(string text);

    /// <summary>
    /// Removes language-specific stop words from the provided token list.
    /// </summary>
    IEnumerable<string> RemoveStopWords(IEnumerable<string> tokens);

    /// <summary>
    /// Calculates word frequencies, filtering out stop words by default.
    /// </summary>
    IDictionary<string, int> GetWordFrequencies(string text, bool filterStopWords = true);

    /// <summary>
    /// Returns the active stop words set for this language analyzer.
    /// </summary>
    IReadOnlySet<string> GetStopWords();
}
