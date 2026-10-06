using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.WebApi.Keyword.TextAnalysis.Common.Models.SEO;

public record GrammarCheckResult(
    bool IsClean,
    int ErrorCount,
    List<GrammarError> Errors
);
