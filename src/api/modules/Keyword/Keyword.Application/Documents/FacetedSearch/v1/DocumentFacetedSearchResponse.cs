using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.WebApi.Keyword.Application.Documents.FacetedSearch.v1;

public sealed record DocumentFacetedSearchResponse(
    int Id,
    string Title,
    List<string> MatchedKeywords,
    List<string> MatchedKeyphrases
)
{
    private double RelevanceScore { get; set; }
};
