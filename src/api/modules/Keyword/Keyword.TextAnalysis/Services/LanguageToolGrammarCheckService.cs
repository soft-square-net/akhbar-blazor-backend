
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using global::FSH.Starter.WebApi.Keyword.TextAnalysis.Common.Models.SEO;
using global::FSH.Starter.WebApi.Keyword.TextAnalysis.Interfaces.Services;
using Shared.Enums;

public class LanguageToolGrammarCheckService : IGrammarCheckService
{
    private readonly HttpClient _httpClient;

    public LanguageToolGrammarCheckService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // Default base address: https://api.languagetool.org/v2/ (or self-hosted container instance)
    }

    public async Task<GrammarCheckResult> CheckAsync(string text, Language language, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(text))
            return new GrammarCheckResult(true, 0, new List<GrammarError>());

        string langCode = language == Language.Arabic ? "ar" : "en-US";

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("text", text),
            new KeyValuePair<string, string>("language", langCode)
        });

        try
        {
            var response = await _httpClient.PostAsync("check", content, ct);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<LanguageToolResponse>(cancellationToken: ct);

            var errors = new List<GrammarError>();
            if (result?.Matches != null)
            {
                foreach (var match in result.Matches)
                {
                    var suggestions = match.Replacements.Select(r => r.Value).Take(3).ToList();
                    string errorText = text.Substring(match.Offset, Math.Min(match.Length, text.Length - match.Offset));

                    errors.Add(new GrammarError(
                        match.Message,
                        errorText,
                        suggestions,
                        match.Offset,
                        match.Length,
                        match.Rule?.Category?.Name ?? "Grammar"
                    ));
                }
            }

            return new GrammarCheckResult(errors.Count == 0, errors.Count, errors);
        }
        catch
        {
            // Fallback response on API failure
            return new GrammarCheckResult(true, 0, new List<GrammarError>());
        }
    }

    // LanguageTool API DTOs
    private class LanguageToolResponse
    {
        [JsonPropertyName("matches")] public List<MatchDto> Matches { get; set; } = new();
    }

    private class MatchDto
    {
        [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;
        [JsonPropertyName("offset")] public int Offset { get; set; }
        [JsonPropertyName("length")] public int Length { get; set; }
        [JsonPropertyName("replacements")] public List<ReplacementDto> Replacements { get; set; } = new();
        [JsonPropertyName("rule")] public RuleDto? Rule { get; set; }
    }

    private class ReplacementDto { [JsonPropertyName("value")] public string Value { get; set; } = string.Empty; }
    private class RuleDto { [JsonPropertyName("category")] public CategoryDto? Category { get; set; } }
    private class CategoryDto { [JsonPropertyName("name")] public string Name { get; set; } = string.Empty; }
}
