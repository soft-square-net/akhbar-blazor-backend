using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Application.Documents.Create.v1;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Document.Application.Documents.Create.v1;
public sealed class CreateDocumentHandler(
    ILogger<CreateDocumentHandler> logger,
    [FromKeyedServices("document:documents")] IRepository<Domain.Document> repository)
    : IRequestHandler<CreateDocumentCommand, CreateDocumentResponse>
{
    public async Task<CreateDocumentResponse> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var document = Domain.Document.Create(request.Name!, request.Description);
        await repository.AddAsync(document, cancellationToken);
        logger.LogInformation("document created {DocumentId}", document.Id);
        return new CreateDocumentResponse(document.Id);
    }
}
