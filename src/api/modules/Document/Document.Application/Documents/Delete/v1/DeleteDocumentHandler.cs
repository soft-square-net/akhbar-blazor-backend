using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Domain;
using FSH.Starter.WebApi.Document.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Document.Application.Documents.Delete.v1;
public sealed class DeleteDocumentHandler(
    ILogger<DeleteDocumentHandler> logger,
    [FromKeyedServices("document:documents")] IRepository<Domain.Document> repository)
    : IRequestHandler<DeleteDocumentCommand>
{
    public async Task Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var document = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = document ?? throw new DocumentNotFoundException(request.Id);
        await repository.DeleteAsync(document, cancellationToken);
        logger.LogInformation("Document with id : {DocumentId} deleted", document.Id);
    }
}
