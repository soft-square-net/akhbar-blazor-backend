using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.AddFile.v1;
public sealed class AddBucketFileHandler(
    ILogger logger, IStorageServiceFactory serviceFactory, 
    IRepository<Bucket> repository
    ) : IRequestHandler<AddBucketFileCommand, AddBucketFileResponse>
{
   
    public async Task<AddBucketFileResponse> Handle(AddBucketFileCommand request, CancellationToken cancellationToken)
    {
        var Bucket = repository.GetByIdAsync(request.BucketId, cancellationToken);
        var service = serviceFactory.GetFileStorageService(StorageProvider.AmazonS3);

        return new AddBucketFileResponse(Guid.NewGuid());
    }
}
