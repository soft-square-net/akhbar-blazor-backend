using Ardalis.Specification;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.Keyword.Domain;

namespace FSH.Starter.WebApi.Keyword.Application.Documents.FacetedSearch.v1;

public class DocumentFacetedSearchSpec : EntitiesByPaginationFilterSpec<Document, DocumentFacetedSearchResponse>
{
    public DocumentFacetedSearchSpec(DocumentFacetedSearchCommand filter)
        : base(filter)
    {
        Query.AsNoTracking();

        // 1. Text Search across Title or Content
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var term = filter.SearchTerm.ToLower();
            Query.Where(d => d.Title.ToLower().Contains(term) || d.Content.ToLower().Contains(term));
        }

        // 2. Facet Filter: Named Entity Types (e.g., Company, Person)
        if (filter.EntityTypes != null && filter.EntityTypes.Any())
        {
            Query.Where(d => d.DocumentKeywords.Any(dk => filter.EntityTypes.Contains(dk.Keyword.EntityType)));
        }

        // 3. Facet Filter: Parts of Speech (e.g., Verb, Adjective)
        if (filter.PartsOfSpeech != null && filter.PartsOfSpeech.Any())
        {
            Query.Where(d => d.DocumentKeywords.Any(dk => filter.PartsOfSpeech.Contains(dk.Keyword.PartOfSpeech)));
        }

        // 4. Facet Filter: Specific Keyword Values
        if (filter.Keywords != null && filter.Keywords.Any())
        {
            var lowerKeywords = filter.Keywords.Select(k => k.ToLower()).ToList();
            Query.Where(d => d.DocumentKeywords.Any(dk => lowerKeywords.Contains(dk.Keyword.Value)));
        }

        // 5. Minimum Relevance/TF-IDF Threshold
        if (filter.MinRelevanceScore.HasValue)
        {
            Query.Where(d => d.DocumentKeywords.Any(dk => dk.RelevanceScore >= filter.MinRelevanceScore.Value));
        }

        // 6. Projections (SELECT DTO)
        Query.Select(d => new DocumentFacetedSearchResponse(d.Id, d.Title,
            d.DocumentKeywords.Select(dk => dk.Keyword.Value).ToList(),
            d.DocumentKeyphrases.Select(dk => dk.Keyphrase.Value).ToList()));

        // 7. Pagination
        int skip = (filter.PageNumber - 1) * filter.PageSize;
        Query.Skip(skip).Take(filter.PageSize);
    }
}
