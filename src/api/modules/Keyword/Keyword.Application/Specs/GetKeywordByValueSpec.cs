
using Ardalis.Specification;

namespace FSH.Starter.WebApi.Keyword.Application.Specs;

internal class GetKeywordByValueSpec: SingleResultSpecification<Domain.Keyword>
{
    public GetKeywordByValueSpec(string value)
    {
        Query.Where(k => k.Value == value);
    }
}
