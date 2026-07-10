using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using FSH.Framework.Core.Persistence;
using MediatR;

namespace FSH.Framework.Core.Localization;

public static class LocalizableEntityExtensions
{
    

    public static IQueryable<TDestination> ProjectToLanguage<TSource, TTranslation, TDestination>(
        this IQueryable<TSource> source,
        string cultureCode)
        where TSource : class, ILocalizableEntity<TTranslation>
        where TTranslation : class, IEntityTranslation
        where TDestination : class, new()
    {
        var sourceType = typeof(TSource);
        var translationType = typeof(TTranslation);
        var destinationType = typeof(TDestination);

        // التعبير البرمجي الأساسي للـ Lambda: (src => ...)
        var sourceParameter = Expression.Parameter(sourceType, "src");

        // الحصول على خاصية الـ Translations من الـ Source
        var translationsProp = sourceType.GetProperty("Translations")!;

        // بناء الـ Expression الخاص بجلب الترجمة المناسبة للغة المطلوبة:
        // src.Translations.FirstOrDefault(t => t.LanguageCode == cultureCode) ?? src.Translations.FirstOrDefault()
        var translationsRoute = Expression.Property(sourceParameter, translationsProp);

        var langParam = Expression.Parameter(translationType, "t");
        var langCodeProp = Expression.Property(langParam, nameof(IEntityTranslation.LanguageCode));
        var cultureTarget = Expression.Constant(cultureCode);
        var langCompare = Expression.Equal(langCodeProp, cultureTarget);
        var langLambda = Expression.Lambda<Func<TTranslation, bool>>(langCompare, langParam);

        // FirstOrDefault المفلتر باللغة
        var firstOrDefaultWithLangMethod = typeof(Enumerable)
            .GetMethods()
            .First(m => m.Name == nameof(Enumerable.FirstOrDefault) && m.GetParameters().Length == 2)
            .MakeGenericMethod(translationType);

        var matchedTranslation = Expression.Call(firstOrDefaultWithLangMethod, translationsRoute, langLambda);

        // FirstOrDefault الافتراضي (Fallback في حال عدم وجود الترجمة المطلوبة)
        var firstOrDefaultMethod = typeof(Enumerable)
            .GetMethods()
            .First(m => m.Name == nameof(Enumerable.FirstOrDefault) && m.GetParameters().Length == 1)
            .MakeGenericMethod(translationType);

        var defaultTranslation = Expression.Call(firstOrDefaultMethod, translationsRoute);

        // الكائن النهائي المختار للترجمة: (matchedTranslation ?? defaultTranslation)
        var translationChoice = Expression.Coalesce(matchedTranslation, defaultTranslation);

        // بناء الـ Member Assignments للـ DTO (TDestination)
        var bindings = new List<MemberAssignment>();
        var destProperties = destinationType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var destProp in destProperties)
        {
            if (!destProp.CanWrite) continue;

            // 1. البحث في جدول التراجم (TTranslation) أولاً بنفس الاسم والنوع
            var translationMatch = translationType.GetProperty(destProp.Name, destProp.PropertyType);
            if (translationMatch != null)
            {
                // الخصائص في جدول الترجمة قد ترجع Null، لذا نتعامل مع الـ Coalesce للـ Fallback من الـ Source إن وجد
                var translationValue = Expression.Property(translationChoice, translationMatch);

                // تحقق إذا كان هناك خاصية بنفس الاسم في الـ Source كـ Fallback افتراضي
                var sourceFallback = sourceType.GetProperty(destProp.Name, destProp.PropertyType);
                if (sourceFallback != null)
                {
                    var sourceValue = Expression.Property(sourceParameter, sourceFallback);
                    var coalesceExpression = Expression.Coalesce(translationValue, sourceValue);
                    bindings.Add(Expression.Bind(destProp, coalesceExpression));
                }
                else
                {
                    bindings.Add(Expression.Bind(destProp, translationValue));
                }
            }
            else
            {
                // 2. إذا لم تكن الخاصية مترجمة، ابحث عنها مباشرة في الـ Source (مثل المعرفات، التواريخ، البولين)
                var sourceMatch = sourceType.GetProperty(destProp.Name, destProp.PropertyType);
                if (sourceMatch != null)
                {
                    var sourceValue = Expression.Property(sourceParameter, sourceMatch);
                    bindings.Add(Expression.Bind(destProp, sourceValue));
                }
            }
        }

        // إنشاء كائن الـ Destination الجديد وعمل الـ Mapping
        var newDestination = Expression.New(destinationType);
        var memberInit = Expression.MemberInit(newDestination, bindings);

        // تحويل الـ Expression Tree بأكملها إلى Lambda يمكن للـ EF Core قراءتها وترجمتها لـ SQL
        var lambda = Expression.Lambda<Func<TSource, TDestination>>(memberInit, sourceParameter);

        return source.Select(lambda);
    }

    //public static IEnumerable<TDestination> ProjectToLanguage<TSource, TTranslation, TDestination>(this IEnumerable<TSource> source, string cultureCode)
    //where TSource : class, ILocalizableEntity<TTranslation>
    //where TTranslation : class, IEntityTranslation
    //where TDestination : class, new()
    //{
    //    var destinationList = new List<TDestination>();
    //    var sourceProps = typeof(TSource).GetProperties();
    //    var translationProps = typeof(TTranslation).GetProperties();
    //    var destProps = typeof(TDestination).GetProperties();
    //    foreach (var item in source)
    //    {
    //        var dto = new TDestination(); // 1. يكيمانيد يبنجألا حاتفملا طبر // يعباتت فذح لمعو يعباتت فذح لمعو )طباورلاو لاقملا فّرعم لثم( ًايئاقلت ةكرتشملا لوقحلا خسن . 
    //        foreach (var destProp in destProps)
    //        {
    //            var sourceProp = sourceProps.FirstOrDefault(p => p.Name == destProp.Name && p.PropertyType == destProp.PropertyType);
    //            if (sourceProp != null) destProp.SetValue(dto, sourceProp.GetValue(item));
    //        } // 2. طلا ةغل عم ةقفاوتملا ةمجرتلا صوصن بلج .2 // راركت مدع نامضل ديرف طلا ةغل عم ةقفاوتملا ةمجرتلا صوصن بلج . 
    //        var translation = item.Translations.FirstOrDefault(t => t.LanguageCode.Equals(cultureCode, StringComparison.OrdinalIgnoreCase));
    //        translation ??= item.Translations.FirstOrDefault(); // Fallback 
    //        if (translation != null)
    //        {
    //            // DTO يعباتت فذح لمعو ا لخاد ةلاعفلا عاونألا لك ىلع لوصحلا . 
    //            foreach (var destProp in destProps)
    //            {
    //                var transProp = translationProps.FirstOrDefault(p => p.Name == destProp.Name && p.PropertyType == destProp.PropertyType);
    //                if (transProp != null)
    //                    destProp.SetValue(dto, transProp.GetValue(translation));
    //            }
    //        }
    //        destinationList.Add(dto);
    //    }
    //    return destinationList;
    //}

}
public class NewsArticleDto
{
    public Guid Id { get; set; }
    public DateTime PublishedOn { get; set; }
    public string AuthorName { get; set; } = default!;

    // سيتم جلبهم ديناميكياً من جدول التراجم بناءً على اللغة
    public string Title { get; set; } = default!;
    public string BodyContent { get; set; } = default!;
}

public class NewsArticle : LocalizableEntity<Guid, NewsArticleTranslation>
{
    public DateTime PublishedOn { get; set; } 
    public string AuthorName { get; set; } = default!;
}

public class NewsArticleTranslation : IEntityTranslation
{
    public Guid Id { get; set; }
    public string LanguageCode { get; set; } = default!; // ar-EG, en-US

    // الحقول المترجمة
    public string Title { get; set; } = default!;
    public string BodyContent { get; set; } = default!;

    public Guid NewsArticleId { get; set; }
}

public class test
{
    public void fun1()
    {
        var articles = new List<NewsArticle>().AsQueryable(); // Assuming you have a list of ArticleCategory
        string currentLang = "en-US"; // Example language code
        var result = articles.ProjectToLanguage<NewsArticle, NewsArticleTranslation, NewsArticleDto>(currentLang);
    }

}


//public class GetNewsArticleHandler : IRequestHandler<GetNewsArticleRequest, NewsArticleDto>
//{
//    private readonly IRepository<NewsArticle> _repository;

//    public GetNewsArticleHandler(IRepository<NewsArticle> repository) => _repository = repository;

//    public async Task<NewsArticleDto> Handle(GetNewsArticleRequest request, CancellationToken cancellationToken)
//    {
//        // جلب كود اللغة الحالية المقروءة من الـ Request Middleware تلقائياً
//        string currentCulture = CultureInfo.CurrentCulture.Name;

//        var articleDto = await _repository.Entities
//            .Where(x => x.Id == request.Id)
//            .ProjectToLanguage<NewsArticle, NewsArticleTranslation, NewsArticleDto>(currentCulture)
//            .FirstOrDefaultAsync(cancellationToken);

//        return articleDto!;
//    }
//}


