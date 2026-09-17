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
    [FromKeyedServices("document:buckets")] IRepository<Bucket> repository,
    [FromKeyedServices("document:files")] IReadRepository<Domain.File> fileRepo
    ) : IRequestHandler<GetBucketFileRequest, GetBucketFileResponse>
{

    public async Task<GetBucketFileResponse> Handle(GetBucketFileRequest request, CancellationToken cancellationToken)
    {
        var bucket = await repository.GetByIdAsync(request.BucketId, cancellationToken);
        var file = await fileRepo.GetByIdAsync(request.FileId, cancellationToken);

        // Get The file actual data 
        // var service = serviceFactory.GetFileStorageService(StorageProvider.AmazonS3);


        return new GetBucketFileResponse(request.BucketId, request.FileId, file.Folder,file.Key, file.Name,file.Description,file.Extension,file.Etag,file.Url,file.FileType,file.Size, file.IsPublic);
    }
}
