using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.WebApi.Keyword.TextAnalysis.Common.Models;

public record DocumentAnalysisMetrics(
    int TotalWordCount,
    int UniqueWordCount,
    int CharacterCount,
    int EstimatedReadingTimeMinutes,
    Dictionary<string, int> KeywordFrequencies,
    Dictionary<string, int> KeyphraseFrequencies);
