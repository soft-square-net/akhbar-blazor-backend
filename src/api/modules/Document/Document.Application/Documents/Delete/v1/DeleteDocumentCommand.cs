using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Documents.Delete.v1;
public sealed record DeleteDocumentCommand(
    Guid Id) : IRequest;
