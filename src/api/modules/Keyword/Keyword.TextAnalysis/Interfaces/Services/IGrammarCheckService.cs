
using FSH.Starter.WebApi.Keyword.TextAnalysis.Common.Models.SEO;
using Shared.Enums;

namespace FSH.Starter.WebApi.Keyword.TextAnalysis.Interfaces.Services;

public interface IGrammarCheckService
{
    Task<GrammarCheckResult> CheckAsync(string text, Language language, CancellationToken ct = default);
}
