using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Documents.Update.v1;
public sealed record UpdateDocumentCommand(
    Guid Id,
    string? Name,
    string? Description = null) : IRequest<UpdateDocumentResponse>;
