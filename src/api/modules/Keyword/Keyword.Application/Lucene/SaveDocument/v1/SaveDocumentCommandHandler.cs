
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Keyword.Application.Lucene.SaveDocument.v1;
using FSH.Starter.WebApi.Keyword.Domain;
using global::FSH.Starter.WebApi.Keyword.TextAnalysis.Common.Models.SEO;
using global::FSH.Starter.WebApi.Keyword.TextAnalysis.Interfaces.Common;
using global::FSH.Starter.WebApi.Keyword.TextAnalysis.Interfaces.Services;

using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace FSH.Starter.Modules.Catalog.Application.Documents.Commands.SaveDocument;



public class SaveDocumentCommandHandler : IRequestHandler<SaveDocumentCommand, SaveDocumentResponse>
{
    private readonly IRepository<SEODocument> _repository;
    private readonly ISeoAnalysisService _seoAnalysisService;
    private readonly IGrammarCheckService _grammarCheckService;
    private readonly IDocumentSearchIndexer _searchIndexer;

    public SaveDocumentCommandHandler(
        [FromKeyedServices("keyword:SEODocuments")] IRepository<SEODocument> repository,
        ISeoAnalysisService seoAnalysisService,
        IGrammarCheckService grammarCheckService,
        IDocumentSearchIndexer searchIndexer)
    {
        _repository = repository;
        _seoAnalysisService = seoAnalysisService;
        _grammarCheckService = grammarCheckService;
        _searchIndexer = searchIndexer;
    }

    public async Task<SaveDocumentResponse> Handle(SaveDocumentCommand request, CancellationToken cancellationToken)
    {
        // 1. Run SEO Analysis (In-Memory Processing)
        SeoAnalysisResult seoResult = _seoAnalysisService.AnalyzeDocument(
            content: request.Content,
            title: request.Title,
            focusKeyword: request.FocusKeyword,
            metaDescription: request.MetaDescription,
            language: request.Language);

        // 2. Run Grammar & Spell Check (External API / Service Call)
        GrammarCheckResult grammarResult = await _grammarCheckService.CheckAsync(
            text: request.Content,
            language: request.Language,
            ct: cancellationToken);

        // 3. Retrieve or Create Entity
        SEODocument document;
        if (request.Id.HasValue && request.Id.Value != Guid.Empty)
        {
            document = await _repository.GetByIdAsync(request.Id.Value, cancellationToken)
                ?? throw new KeyNotFoundException($"Document with ID {request.Id} was not found.");

            document.Update(
                title: request.Title,
                content: request.Content,
                focusKeyword: request.FocusKeyword,
                metaDescription: request.MetaDescription,
                language: request.Language,
                seoScore: seoResult.OverallScore,
                grammarErrorCount: grammarResult.ErrorCount);
        }
        else
        {
            document = SEODocument.Create(
                title: request.Title,
                content: request.Content,
                focusKeyword: request.FocusKeyword,
                metaDescription: request.MetaDescription,
                language: request.Language,
                seoScore: seoResult.OverallScore,
                grammarErrorCount: grammarResult.ErrorCount);

            await _repository.AddAsync(document, cancellationToken);
        }

        // 4. Save Changes to Database
        await _repository.SaveChangesAsync(cancellationToken);

        // 5. Index Document into Lucene Search Engine
        await _searchIndexer.IndexDocumentAsync(
            documentId: document.Id,
            title: document.Title,
            content: document.Content,
            language: document.Language,
            ct: cancellationToken);

        // 6. Return Aggregated Response
        return new SaveDocumentResponse(
            DocumentId: document.Id,
            SeoAnalysis: seoResult,
            GrammarCheck: grammarResult,
            SavedAtUtc: DateTime.UtcNow);
    }
}
