
using Shared.Enums;

namespace FSH.Starter.WebApi.Keyword.TextAnalysis.Interfaces.Common;

public interface IDocumentSearchIndexer
{
    Task IndexDocumentAsync(
        Guid documentId,
        string title,
        string content,
        Language language,
        CancellationToken ct = default);
}
