
using MediatR;
using Shared.Enums;

namespace FSH.Starter.WebApi.Keyword.Application.Lucene.SaveDocument.v1;

public record SaveDocumentCommand(
    Guid? Id,
    string Title,
    string Content,
    string? FocusKeyword,
    string? MetaDescription,
    Language Language = Language.English) : IRequest<SaveDocumentResponse>;
