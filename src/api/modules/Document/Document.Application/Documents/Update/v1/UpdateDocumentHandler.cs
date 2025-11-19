using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Domain;
using FSH.Starter.WebApi.Document.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Document.Application.Documents.Update.v1;
public sealed class UpdateDocumentHandler(
    ILogger<UpdateDocumentHandler> logger,
    [FromKeyedServices("document:documents")] IRepository<Domain.Document> repository)
    : IRequestHandler<UpdateDocumentCommand, UpdateDocumentResponse>
{
    public async Task<UpdateDocumentResponse> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var document = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = document ?? throw new DocumentNotFoundException(request.Id);
        var updatedDocument = document.Update(request.Name, request.Description);
        await repository.UpdateAsync(updatedDocument, cancellationToken);
        logger.LogInformation("Document with id : {DocumentId} updated.", document.Id);
        return new UpdateDocumentResponse(document.Id);
    }
}
