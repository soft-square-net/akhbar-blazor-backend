
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Shared.Enums;

namespace FSH.Starter.WebApi.Keyword.Domain;

public class SEODocument: AuditableEntity<Guid>, IAggregateRoot
{
    private SEODocument(string title,
               string content,
               string focusKeyword,
               string metaDescription,
               Language language,
               float seoScore,
               int grammarErrorCount)
    {
        Title = title;
        Content = content;
        FocusKeyword = focusKeyword;
        MetaDescription = metaDescription;
        Language = language;
        SeoScore = seoScore;
        GrammarErrorCount = grammarErrorCount;
    }

    public string Title { get; set; }
    public string Content { get; set; }
    public string FocusKeyword { get; set; }
    public string MetaDescription { get; set; }
    public Language Language { get; set; }
    public float SeoScore { get; set; }
    public int GrammarErrorCount { get; set; }

    public static SEODocument Create(
               string title,
               string content,
               string focusKeyword,
               string metaDescription,
               Language language,
               float seoScore,
               int grammarErrorCount)
    { 
        return new SEODocument(title, content, focusKeyword, metaDescription, language, seoScore, grammarErrorCount); 
    }

    public SEODocument Update(
                string title,
                string content,
                string focusKeyword,
                string metaDescription,
                Language language,
                float seoScore,
                int grammarErrorCount) { return this; }
}
