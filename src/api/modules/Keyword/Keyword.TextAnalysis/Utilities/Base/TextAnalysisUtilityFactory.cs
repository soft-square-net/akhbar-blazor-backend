using Shared.Enums;
namespace FSH.Starter.WebApi.Keyword.TextAnalysis.Utilities.Base;


public static class TextAnalysisUtilityFactory
{
    public static ITextAnalysisUtility Create(Language language) => language switch
    {
        Language.Arabic => new ArabicTextAnalysisUtility(),
        Language.English => new EnglishTextAnalysisUtility(),
        _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
    };
}
