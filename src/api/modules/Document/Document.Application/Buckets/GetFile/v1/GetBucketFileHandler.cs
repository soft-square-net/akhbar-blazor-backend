using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.GetFile.v1;
public sealed class GetBucketFileHandler(
    ILogger<GetBucketFileHandler> logger, IStorageServiceFactory serviceFactory, 
    [FromKeyedServices("document:buckets")] IRepository<Bucket> repository
    ) : IRequestHandler<GetBucketFileRequest, GetBucketFileResponse>
{
   
    public async Task<GetBucketFileResponse> Handle(GetBucketFileRequest request, CancellationToken cancellationToken)
    {
        var Bucket = repository.GetByIdAsync(request.BucketId, cancellationToken);
        var service = serviceFactory.GetFileStorageService(StorageProvider.AmazonS3);

        
        return new GetBucketFileResponse(Stream.Null);
    }
}
