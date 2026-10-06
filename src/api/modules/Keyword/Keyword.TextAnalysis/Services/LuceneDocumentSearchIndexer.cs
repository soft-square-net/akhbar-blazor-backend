


using System;
using System.Threading;
using System.Threading.Tasks;
using FSH.Starter.WebApi.Keyword.TextAnalysis.Interfaces.Common;
using Shared.Enums;

namespace FSH.Starter.WebApi.Keyword.TextAnalysis.Services;

public class LuceneDocumentSearchIndexer : IDocumentSearchIndexer
{
    private readonly LuceneSearchService _luceneSearchService;

    public LuceneDocumentSearchIndexer(LuceneSearchService luceneSearchService)
    {
        _luceneSearchService = luceneSearchService;
    }

    public Task IndexDocumentAsync(
        Guid documentId,
        string title,
        string content,
        Language language,
        CancellationToken ct = default)
    {
        // Map Guid to internal integer or string identifier for Lucene indexing
        _luceneSearchService.IndexDocument(
            id: documentId.GetHashCode(),
            title: title,
            content: content,
            language: language);

        return Task.CompletedTask;
    }
}
