namespace FSH.Starter.WebApi.Keyword.Application.Documents.FacetedSearch.v1;

public class FacetItemDto<T>
{
    public T Value { get; set; } = default!;
    public string Label { get; set; } = string.Empty;
    public int Count { get; set; }
}
