using System.Text.RegularExpressions;

namespace FSH.Starter.WebApi.Keyword.TextAnalysis.Utilities.Base;

public class ArabicTextAnalysisUtility : ITextAnalysisUtility
{
    private static readonly Regex TashkeelRegex = new Regex(@"[\u064B-\u0652\u0640]", RegexOptions.Compiled);
    private static readonly Regex PunctuationRegex = new Regex(@"[^\w\s]", RegexOptions.Compiled);

    private readonly HashSet<string> _stopWords;

    public ArabicTextAnalysisUtility(IEnumerable<string>? customStopWords = null)
    {
        _stopWords = customStopWords != null
            ? new HashSet<string>(customStopWords.Select(Normalize))
            : new HashSet<string>(DefaultArabicStopWords.Select(Normalize));
    }

    public string Normalize(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;

        // 1. Remove Tashkeel (diacritics) and Tatweel (kashida)
        text = TashkeelRegex.Replace(text, string.Empty);

        // 2. Normalize Alef forms (أ, إ, آ -> ا)
        text = Regex.Replace(text, @"[أإآ]", "ا");

        // 3. Normalize Taa Marbuta (ة -> ه) and Yaa (ى -> ي)
        text = text.Replace('ة', 'ه').Replace('ى', 'ي');

        return text.Trim();
    }

    public IEnumerable<string> Tokenize(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return Enumerable.Empty<string>();

        string cleaned = PunctuationRegex.Replace(text, " ");
        return cleaned.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                      .Select(Normalize);
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

    private static readonly HashSet<string> DefaultArabicStopWords = new()
    {
        "من", "في", "على", "إلى", "عن", "مع", "حتى", "إذا", "أن", "إن", "أو", "ثم", "بل", "لا", "ما",
        "هل", "لم", "لن", "كان", "كانت", "يكون", "هذا", "هذه", "هؤلاء", "ذلك", "تلك", "الذي", "التي",
        "الذين", "هو", "هي", "هم", "هن", "أنا", "نحن", "أنت", "أنتما", "أنتم", "أنتن", "عند", "بعد",
        "قبل", "بين", "تحت", "فوق", "غير", "كل", "بعض", "جميع", "تم", "قام", "أجل", "فإن"
    };





    // 4. Arabic Stop Words Categorized
    //Below is a categorized list of standard Arabic stop words (كلمات الإيقاف).

    //Prepositions & Particles (حروف الجر والعطف والقطع)
    //من · في · على · إلى · عن · مع · حتى · الباء (بـ) · الكاف (كـ) · اللام (لـ) · رب · مذ · منذ · خلا · عدا · حاشا · ثم · أو · أم · بل · لكن · فـ · وـ

    //Demonstrative Pronouns (أسماء الإشارة)
    //هذا · هذه · هذان · هاتان · هؤلاء · ذلك · تلك · ذانك · تانك · أولئك · هنا · هناك · هنالك

    //Relative Pronouns (الأسماء الموصولة)
    //الذي · التي · اللذان · اللتان · الذين · اللاتي · اللواتي · اللاائي · من (الموصولة) · ما (الموصولة)

    //Personal Pronouns (الضمائر المنفصلة)
    //أنا · نحن · أنت · أنتِ · أنتما · أنتم · أنتن · هو · هي · هما · هم · هن · إياي · إيانا · إياك · إياه

    //Auxiliary Verbs & Negation (الأفعال الناقصة والأدوات)
    //كان · كانت · يكون · تكون · كانوا · أصبح · أمست · ظل · بات · صار · ليس · ليست · لا · ما · لم · لن · إنما · غير · سوى · تم · قام

    //Interrogative & Adverbs (أسماء الاستفهام والظروف)
    //هل · كيف · متى · أين · كم · ماذا · لماذا · منذ · حين · عند · بعد · قبل · بين · تحت · فوق · أمام · خلف · معظم · كل · بعض · جميع


}
