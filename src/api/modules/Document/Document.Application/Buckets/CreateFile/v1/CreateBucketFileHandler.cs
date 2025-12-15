using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Starter.WebApi.Document.Appication.Buckets.Specs;
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
        // var bucket = await repository.SingleOrDefaultAsync(new BucketNavigateToFoldersSpec(request.BucketId), cancellationToken);

        var bucket = await repository.SingleOrDefaultAsync(new GetBucketByIdSpec(request.BucketId), cancellationToken);
        var parentFolder = bucket.Folders.FirstOrDefault(f => f.Id == request.ParentFolderId);
        // var Key = string.IsNullOrEmpty(parentFolder?.FullPath) ? request.FileName : $"{parentFolder?.FullPath.TrimStart('/').TrimEnd('/')}/{request.FileName}";
        var service = serviceFactory.GetFileStorageService(StorageProvider.AmazonS3);
        var filenExtension = request.FileName.Split(".").Last();
        string Key = await service.UploadFileAsync(request.FileContent, bucket.Name, request.FileName, request.ContentType, request.FileType, filenExtension, parentFolder?.FullPath,bucket.StorageAccount.AccessKey,bucket.StorageAccount.SecretKey, cancellationToken);
        logger.LogInformation("File \"{Key}\" created in bucket {BucketId}", Key, request.BucketId);
        String uri = String.Format("s3://{0}/{1}/{3}", bucket.Name,parentFolder?.FullPath.TrimStart('/'), request.FileName);
        String objectArn = String.Format("arn:aws:s3:::{0}/{1}/{2}", bucket.Name,parentFolder?.FullPath.TrimStart('/'), request.FileName);
        bucket.AddFile(parentFolder?.Id ?? bucket.Folders[0].Id, Key, request.FileName, filenExtension, objectArn, request.FileType, request.FileSize, false, "");
        await repository.UpdateAsync(bucket, cancellationToken);
        return new CreateBucketFileResponse(bucket.Id);
    }
}
