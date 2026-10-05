//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Shared.Enums;

//namespace FSH.Starter.WebApi.Keyword.Application.Documents.FacetedSearch.v1;

//public class DocumentSearchFilter
//{
//    public string? SearchTerm { get; set; }

//    // Facets
//    public List<NamedEntityType>? EntityTypes { get; set; } // e.g., [Company, Person]
//    public List<PartOfSpeech>? PartsOfSpeech { get; set; }  // e.g., [Noun, Adjective]
//    public List<string>? Keywords { get; set; }            // e.g., ["microsoft", "fast"]
//    public float? MinRelevanceScore { get; set; }

//    // Pagination & Sorting
//    public int PageNumber { get; set; } = 1;
//    public int PageSize { get; set; } = 10;
//    public string? SortBy { get; set; } // e.g., "title", "relevance"
//}
