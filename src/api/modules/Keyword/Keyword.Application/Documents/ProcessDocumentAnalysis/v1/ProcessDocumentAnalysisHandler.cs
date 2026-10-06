using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Keyword.Application.Specs;
using FSH.Starter.WebApi.Keyword.Domain;
using FSH.Starter.WebApi.Keyword.TextAnalysis.Utilities.Base;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

using Shared.Enums;

namespace FSH.Starter.WebApi.Keyword.Application.Documents.ProcessDocumentAnalysis.v1;

public sealed class ProcessDocumentAnalysisHandler(
    [FromKeyedServices("keyword:Documents")]
    IReadRepository<Document> repository,
    [FromKeyedServices("keyword:Keywords")]
    IRepository<Domain.Keyword> keywordRepository,
    [FromKeyedServices("keyword:DocumentKeywords")]
    IRepository<DocumentKeyword> documentKeywordRepository) :
    IRequestHandler<ProcessDocumentAnalysisCommand, bool>
{

    public async Task<bool> Handle(ProcessDocumentAnalysisCommand request, CancellationToken cancellationToken)
    {
        var document =  await repository.GetByIdAsync(request.DocumentId, cancellationToken);

        if (document == null) return false;

        // 1. Run Text Analysis Utility over document content
        var metrics = TextAnalysisUtility.AnalyzeContent($"{document.Title} {document.Content}");
        int totalCorpusDocuments = await repository.CountAsync(cancellationToken);

        // 2. Process Keywords and calculate TF-IDF
        foreach (var (wordValue, frequency) in metrics.KeywordFrequencies)
        {
            // Find or create global Keyword entity
            var keyword = await keywordRepository
                .SingleOrDefaultAsync(new GetKeywordByValueSpec(wordValue), cancellationToken);

            if (keyword == null)
            {
                keyword = new Domain.Keyword { Value = wordValue, PartOfSpeech = PartOfSpeech.Unknown };
                await keywordRepository.AddAsync(keyword, cancellationToken);
                await keywordRepository.SaveChangesAsync(cancellationToken); // Save to get the ID
            }

            // Count how many documents contain this keyword for IDF
            int docsWithKeyword = await documentKeywordRepository
                .CountAsync(new GetDocumentKeywordsbyKeywordIdSpec(keyword.Id), cancellationToken) + 1;

            // Calculate TF-IDF Relevance Score
            float relevanceScore = TextAnalysisUtility.CalculateTfIdfScore(
                termCountInDoc: frequency,
                totalWordsInDoc: metrics.TotalWordCount,
                totalDocuments: totalCorpusDocuments,
                documentsWithTerm: docsWithKeyword
            );

            // Upsert Junction Record
            await documentKeywordRepository.AddAsync(new DocumentKeyword
            {
                DocumentId = document.Id,
                KeywordId = keyword.Id,
                Frequency = frequency,
                RelevanceScore = relevanceScore
            }, cancellationToken);
        }

        await documentKeywordRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
