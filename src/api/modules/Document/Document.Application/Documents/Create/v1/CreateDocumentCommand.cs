using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Documents.Create.v1;
public sealed record CreateDocumentCommand(
    [property: DefaultValue("Sample Document")] string? Name,
    [property: DefaultValue("Descriptive Description")] string? Description = null) : IRequest<CreateDocumentResponse>;

