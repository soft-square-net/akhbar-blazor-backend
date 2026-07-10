using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Framework.Core.Domain;
using Mapster;

namespace FSH.Framework.Core.Localization;

public abstract class LocalizableEntity<TID,TTranslation> : AuditableEntity<TID>, ILocalizableEntity<TTranslation>
    where TTranslation : class, IEntityTranslation
{
    public virtual ICollection<TTranslation> Translations { get; set; } = new List<TTranslation>();
}


//public abstract class LocalizableEntity<TEntityTranslation, TID> : AuditableEntity<TID>, ILocalizableEntity<TEntityTranslation> where TID : class where TEntityTranslation: IEntityTranslation
//{
//    public ICollection<TEntityTranslation> Translations { get; set; }


//    // public IQueryable<Dto> SelectInCurrentLanguage<Dto>(this IQueryable<EntityTranslation> query) where Dto : class, new()
//    public virtual IQueryable<Dto> SelectInCurrentLanguage<Dto>() where Dto : class, new()
//    {
//        string currentLang = CultureInfo.CurrentCulture.Name; // جلب اللغة من الـ Thread الحالي

//        //return query.Select(c => new Dto
//        //{
//        //    Id = c.Id,
//        //    IsActive = c.IsActive,
//        //    // محاولة جلب اللغة الحالية، وإذا لم توجد نأخذ اللغة الافتراضية (الأولى مثلاً)
//        //    Name = c.Translations.FirstOrDefault(t => t.LanguageCode == currentLang).Name
//        //           ?? c.Translations.FirstOrDefault().Name,
//        //    Description = c.Translations.FirstOrDefault(t => t.LanguageCode == currentLang).Description
//        //                  ?? c.Translations.FirstOrDefault().Description
//        //});
//        return Translations.Select(c => c.Adapt<Dto>()).AsQueryable();
//    }
//}
