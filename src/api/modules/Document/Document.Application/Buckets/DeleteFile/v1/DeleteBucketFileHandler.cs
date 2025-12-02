using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using FSH.Starter.WebApi.Document.Appication.Buckets.DeleteFile.v1;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.AddFile.v1;
public sealed class DeleteBucketFileHandler(
    ILogger<DeleteBucketFileHandler> logger, IStorageServiceFactory serviceFactory, 
    [FromKeyedServices("document:buckets")] IRepository<Bucket> repository
    ) : IRequestHandler<DeleteBucketFileCommand, DeleteBucketFileResponse>
{
   
    public async Task<DeleteBucketFileResponse> Handle(DeleteBucketFileCommand request, CancellationToken cancellationToken)
    {
        var Bucket = repository.GetByIdAsync(request.BucketId, cancellationToken);
        var service = serviceFactory.GetFileStorageService(StorageProvider.AmazonS3);

        return new DeleteBucketFileResponse(Guid.NewGuid(), Guid.NewGuid());
    }
}
