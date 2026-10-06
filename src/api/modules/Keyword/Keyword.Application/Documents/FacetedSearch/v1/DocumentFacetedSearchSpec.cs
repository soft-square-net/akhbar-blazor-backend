using System.Linq.Expressions;
using Ardalis.Specification;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.Keyword.Domain;

namespace FSH.Starter.WebApi.Keyword.Application.Documents.FacetedSearch.v1;

public class DocumentFacetedSearchSpec : EntitiesByPaginationFilterSpec<Document, DocumentFacetedSearchResponse>
{
    public DocumentFacetedSearchSpec(DocumentFacetedSearchCommand filter,
        Expression<Func<Document, bool>>? textSearchExpression,
        Expression<Func<Document, double>>? rankExpression)
        : base(filter)
    {
        Query.AsNoTracking();

        // 1. Text Search across Title or Content
        // 1. Inject Provider-Specific Text Search Expression
        if (textSearchExpression != null)
        {
            Query.Where(textSearchExpression);
        }

        // 2. Facet Filter: Named Entity Types (e.g., Company, Person)
        if (filter.EntityTypes?.Any() == true)
        {
            Query.Where(d => d.DocumentKeywords.Any(dk => filter.EntityTypes.Contains(dk.Keyword.EntityType)));
        }

        // 3. Facet Filter: Parts of Speech (e.g., Verb, Adjective)
        if (filter.PartsOfSpeech?.Any() == true)
        {
            Query.Where(d => d.DocumentKeywords.Any(dk => filter.PartsOfSpeech.Contains(dk.Keyword.PartOfSpeech)));
        }

        // 4. Facet Filter: Specific Keyword Values
        if (filter.Keywords?.Any() == true)
        {
            var lowerKeywords = filter.Keywords.Select(k => k.ToLower()).ToList();
            Query.Where(d => d.DocumentKeywords.Any(dk => lowerKeywords.Contains(dk.Keyword.Value)));
        }


        // 5. Minimum Relevance/TF-IDF Threshold
        if (filter.MinRelevanceScore.HasValue)
        {
            Query.Where(d => d.DocumentKeywords.Any(dk => dk.RelevanceScore >= filter.MinRelevanceScore.Value));
        }
// 3. Relevance Ranking & Order
        if (rankExpression != null)
        {
            Query.OrderByDescending(ConvertToReturnObject(rankExpression));
        }
        else
        {
            Query.OrderByDescending(d => d.Id); // Default sort
        }
        // 6. Projections (SELECT DTO)
        Query.Select(d => new DocumentFacetedSearchResponse(
                d.Id,
                d.Title,
                d.DocumentKeywords.Select(dk => dk.Keyword.Value).ToList(),
                d.DocumentKeyphrases.Select(dk => dk.Keyphrase.Value).ToList()
            )
        );

        // 7. Pagination
        int skip = (filter.PageNumber - 1) * filter.PageSize;
        Query.Skip(skip).Take(filter.PageSize);
    }
    
    public static Expression<Func<T, object?>> ConvertToReturnObject<T, TResult>(Expression<Func<T, TResult>> expression)
    {
        // Wrap the body of the expression in a Convert expression to object
        var convertedBody = Expression.Convert(expression.Body, typeof(object));
    
        // Rebuild the expression with the original parameters
        return Expression.Lambda<Func<T, object?>>(convertedBody, expression.Parameters);
    }

}
