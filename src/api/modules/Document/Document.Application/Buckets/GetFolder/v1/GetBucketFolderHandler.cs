using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.GetFolder.v1;
public sealed class GetBucketFolderHandler(
    ILogger<GetBucketFolderHandler> logger, IStorageServiceFactory serviceFactory, 
    [FromKeyedServices("document:buckets")] IRepository<Bucket> repository
    ) : IRequestHandler<GetBucketFolderRequest, GetBucketFolderResponse>
{
   
    public async Task<GetBucketFolderResponse> Handle(GetBucketFolderRequest request, CancellationToken cancellationToken)
    {
        var Bucket = repository.GetByIdAsync(request.BucketId, cancellationToken);
        var service = serviceFactory.GetFileStorageService(StorageProvider.AmazonS3);

        return new GetBucketFolderResponse(Guid.NewGuid());
    }
}
