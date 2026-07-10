using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;

namespace FSH.Framework.Core.Localization;


public class ArticleDto { }
public interface ILocalizableEntity<TTranslation> where TTranslation : IEntityTranslation
{
    ICollection<TTranslation> Translations { get; set; }

//    private async Task<List<ArticleDto>> Handle (GetActiveArticlesQuery request, CancellationToken cancellationToken ) 
//    { string currentLang =
//_langService.GetCurrentLanguage(); // 2. طلا ةغل عم ةقفاوتملا ةمجرتلا صوصن بلج .2 // نايبلا بلج // طلا ةغل عم ةقفاوتملا ةمجرتلا صوصن بلج .2 // طلا ةغل عم ةقفاوتملا ةمجرتلا صوصن بلجvar articles = await يعباتت فذح لمعو يعباتت فذح لمعو يعباتت فذح لمعو

//    _repository.DbContext.Articles.Include(a => a.Translations ) . Where(a => 
//a.IsActive ) . ToListAsync(cancellationToken ); يعباتت فذح لمعو  , var result = articles.ProjectToLanguage < Article, ArticleTranslation ًايكيمانيد يبنجألا حاتفملا طب
//ر
//ArticleDto
}





///// <summary>
///// /////////////////////////////////////////////////////////////////////////////////////////////
///// </summary>
//public class GetCategoryHandler : IRequestHandler<GetCategoryRequest, CategoryDto>
//{
//    private readonly IRepository<ArticleCategory> _repository;

//    public GetCategoryHandler(IRepository<ArticleCategory> repository) => _repository = repository;

//    public async Task<CategoryDto> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
//    {
//        var category = await _repository.Entities
//            .Where(x => x.Id == request.Id)
//            .SelectInCurrentLanguage<CategoryDto>() // الـ Extension Method السحرية هنا
//            .FirstOrDefaultAsync(cancellationToken);

//        _ = category ?? throw new NotFoundException("Category not found");

//        return category;
//    }
//}
//public class GetCategoryHandler : IRequestHandler<GetCategoryRequest, CategoryDto>
//{
//    private readonly IRepository<ArticleCategory> _repository;

//    public GetCategoryHandler(IRepository<ArticleCategory> repository) => _repository = repository;

//    public async Task<CategoryDto> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
//    {
//        var category = await _repository.Entities
//            .Where(x => x.Id == request.Id)
//            .SelectInCurrentLanguage<CategoryDto>() // الـ Extension Method السحرية هنا
//            .FirstOrDefaultAsync(cancellationToken);

//        _ = category ?? throw new NotFoundException("Category not found");

//        return category;
//    }
//}
