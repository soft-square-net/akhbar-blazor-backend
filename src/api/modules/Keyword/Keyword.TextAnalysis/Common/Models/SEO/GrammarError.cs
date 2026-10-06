using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.WebApi.Keyword.TextAnalysis.Common.Models.SEO;

public record GrammarError(
    string Message,
    string OriginalText,
    List<string> ReplacementSuggestions,
    int Offset,
    int Length,
    string Category
);
