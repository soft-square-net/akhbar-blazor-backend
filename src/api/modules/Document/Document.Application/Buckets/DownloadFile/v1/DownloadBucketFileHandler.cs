using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File.Features;
using FSH.Starter.WebApi.Document.Application.Buckets.Create.v1;
using FSH.Starter.WebApi.Document.Application.Buckets.Specs;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Enums;
using File = FSH.Starter.WebApi.Document.Domain.File;
using Folder = FSH.Starter.WebApi.Document.Domain.Folder;

namespace FSH.Starter.WebApi.Document.Application.Buckets.DownloadFile.v1;
public sealed class DownloadBucketFileHandler(
    ILogger<DownloadBucketFileHandler> logger, IStorageServiceFactory serviceFactory, 
    [FromKeyedServices("document:buckets")] IRepository<Bucket> repository,
    [FromKeyedServices("document:files")] IReadRepository<File> fileRepository
    ) : IRequestHandler<DownloadBucketFileRequest, DownloadBucketFileResponse>
{
   
    public async Task<DownloadBucketFileResponse> Handle(DownloadBucketFileRequest request, CancellationToken cancellationToken)
    {
        var bucket = await repository.SingleOrDefaultAsync(new BucketNavigateToFoldersSpec(request.BucketId), cancellationToken);
        var file = await fileRepository.GetByIdAsync(request.FileId, cancellationToken);
        var service = serviceFactory.GetFileStorageService(StorageProvider.AmazonS3);
        FileDownloadResponse res = await service.DownloadFileAsync(bucket.Name, file.Key, bucket.StorageAccount.AccessKey,
            bucket.StorageAccount.SecretKey, cancellationToken);
       
        var memoryStream = new MemoryStream();
        await res.fileStream.CopyToAsync(memoryStream);
    
        // 3. Rewind the stream position back to the beginning safely
        memoryStream.Position = 0; 
        return new DownloadBucketFileResponse(memoryStream, res.contentType, res.contentLanguage,res.contentDisposition, res.contentMD5, res.contentLength, res.expires);
        // return new DownloadBucketFileResponse(memoryStream);
    }
}
