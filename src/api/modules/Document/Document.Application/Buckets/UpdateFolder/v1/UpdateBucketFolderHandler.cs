using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.UpdateFolder.v1;
public sealed class UpdateBucketFolderHandler(
    ILogger<UpdateBucketFolderHandler> logger, IStorageServiceFactory serviceFactory, 
    [FromKeyedServices("document:buckets")] IRepository<Bucket> repository
    ) : IRequestHandler<UpdateBucketFolderCommand, UpdateBucketFolderResponse>
{
   
    public async Task<UpdateBucketFolderResponse> Handle(UpdateBucketFolderCommand request, CancellationToken cancellationToken)
    {
        var Bucket = repository.GetByIdAsync(request.BucketId, cancellationToken);
        var service = serviceFactory.GetFileStorageService(StorageProvider.AmazonS3);

        return new UpdateBucketFolderResponse(Guid.NewGuid());
    }
}
