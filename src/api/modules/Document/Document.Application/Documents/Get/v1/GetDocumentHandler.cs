using Microsoft.Extensions.DependencyInjection;
using FSH.Starter.WebApi.Document.Domain.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Caching;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Documents.Get.v1;
public sealed class GetDocumentHandler(
    [FromKeyedServices("document:documents")] IReadRepository<Domain.Document> repository,
    ICacheService cache)
    : IRequestHandler<GetDocumentRequest, DocumentResponse>
{
    public async Task<DocumentResponse> Handle(GetDocumentRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"document:{request.Id}",
            async () =>
            {
                var brandItem = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (brandItem == null) throw new DocumentNotFoundException(request.Id);
                return new DocumentResponse(brandItem.Id, brandItem.Name, brandItem.Description);
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
