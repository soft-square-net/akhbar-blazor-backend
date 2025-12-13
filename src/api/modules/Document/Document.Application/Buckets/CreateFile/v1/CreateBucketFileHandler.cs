using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using FSH.Starter.WebApi.Document.Appication.Buckets.Create.v1;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.CreateFile.v1;
public sealed class CreateBucketFileHandler(
    ILogger<CreateBucketFileHandler> logger, IStorageServiceFactory serviceFactory, 
    [FromKeyedServices("document:buckets")] IRepository<Bucket> repository
    ) : IRequestHandler<CreateBucketFileCommand, CreateBucketFileResponse>
{
   
    public async Task<CreateBucketFileResponse> Handle(CreateBucketFileCommand request, CancellationToken cancellationToken)
    {
        var Bucket = await repository.SingleOrDefaultAsync(new BucketNavigateToFoldersSpec(request.BucketId), cancellationToken);
        // var Bucket = await repository.GetByIdAsync(request.BucketId, cancellationToken);
        var service = serviceFactory.GetFileStorageService(StorageProvider.AmazonS3);
        var filenExtension = request.FileName.Split(".").Last();
        string fileUrl = await service.UploadFileAsync(request.FileContent, Bucket.Name, request.FileName, request.ContentType, request.FileType, filenExtension, Bucket.Folders.First()?.Parent?.FullPath ?? "",Bucket.StorageAccount.AccessKey,Bucket.StorageAccount.SecretKey, cancellationToken);
        logger.LogInformation("File created in bucket {BucketId}", request.BucketId);
        Domain.File dbFile =  Domain.File.Create(Bucket.Folders.First(), Guid.NewGuid().ToString(), request.FileName, filenExtension, fileUrl, request.FileType, request.FileSize, false, "");
        
        //Bucket.Folders.First().AddFile(dbFile);
        await repository.UpdateAsync(Bucket, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return new CreateBucketFileResponse(dbFile.Id);
    }
}
