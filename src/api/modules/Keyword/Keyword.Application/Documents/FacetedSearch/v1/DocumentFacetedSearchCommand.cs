using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FSH.Framework.Core.Paging;
using MediatR;
using Shared.Enums;

namespace FSH.Starter.WebApi.Keyword.Application.Documents.FacetedSearch.v1;

public sealed class DocumentFacetedSearchCommand: PaginationFilter,IRequest<DocumentFacetedSearchResponse>, IRequest<PagedList<DocumentFacetedSearchResponse>>
{
    public string? SearchTerm { get; init; } 
    public List<NamedEntityType>? EntityTypes { get; init; } 
    public List<PartOfSpeech>? PartsOfSpeech { get; init; } 
    public List<string>? Keywords { get; init; }
    public float? MinRelevanceScore { get; init; }
    public string? SortBy { get; init; }
}
