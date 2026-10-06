
using Ardalis.Specification;

namespace FSH.Starter.WebApi.Keyword.Application.Specs;

internal class GetDocumentKeywordsbyKeywordIdSpec : Specification<Domain.DocumentKeyword>
{
    public GetDocumentKeywordsbyKeywordIdSpec(int KeywordId)
    {
        Query.Where(dk => dk.KeywordId == KeywordId);
    }
}
