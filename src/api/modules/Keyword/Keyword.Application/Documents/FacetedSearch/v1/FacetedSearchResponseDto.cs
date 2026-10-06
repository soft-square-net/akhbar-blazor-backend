using Shared.Enums;

namespace FSH.Starter.WebApi.Keyword.Application.Documents.FacetedSearch.v1;

public class FacetedSearchResponseDto
{
    public List<DocumentFacetedSearchResponse> Documents { get; set; } = new();
    public int TotalCount { get; set; }

    // Aggregated Facet Breakdown
    public List<FacetItemDto<NamedEntityType>> EntityTypeFacets { get; set; } = new();
    public List<FacetItemDto<PartOfSpeech>> PartOfSpeechFacets { get; set; } = new();
    public List<FacetItemDto<string>> TopKeywordFacets { get; set; } = new();
}
