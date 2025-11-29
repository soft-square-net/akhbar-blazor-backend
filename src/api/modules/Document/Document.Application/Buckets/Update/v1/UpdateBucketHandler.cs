using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Domain;
using FSH.Starter.WebApi.Document.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Document.Application.Buckets.Update.v1;
public sealed class UpdateBucketHandler(
    ILogger<UpdateBucketHandler> logger,
    [FromKeyedServices("document:buckets")] IRepository<Bucket> repository)
    : IRequestHandler<UpdateBucketCommand, UpdateBucketResponse>
{
    public async Task<UpdateBucketResponse> Handle(UpdateBucketCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var bucket = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = bucket ?? throw new BucketNotFoundException(request.Id);
        var updatedDocument = bucket.Update(request.Name, request.Description);
        await repository.UpdateAsync(updatedDocument, cancellationToken);
        logger.LogInformation("Document with id : {DocumentId} updated.", bucket.Id);
        return new UpdateBucketResponse(bucket.Id);
    }
}
