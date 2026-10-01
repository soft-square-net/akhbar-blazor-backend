using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using FSH.Starter.WebApi.Document.Application.Buckets.Specs;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Enums;
using Folder = FSH.Starter.WebApi.Document.Domain.Folder;

namespace FSH.Starter.WebApi.Document.Application.Buckets.RenameFolder.v1;
public sealed class RenameBucketFolderHandler(
        ILogger<RenameBucketFolderHandler> logger,
        IStorageServiceFactory serviceFactory,
        [FromKeyedServices("document:buckets")] IRepository<Bucket> repository
    ) : IRequestHandler<RenameBucketFolderCommand,RenameBucketFolderResponse> {


    public async Task<RenameBucketFolderResponse> Handle(RenameBucketFolderCommand request, CancellationToken cancellationToken)
    {
        var bucket = await repository.FirstOrDefaultAsync(new GetBucketByIdSpec(request.BucketId));
        Folder folder = bucket.Folders.FirstOrDefault(f => f.Id == request.FolderId);  //?? throw new Exception($"Folder with id {request.FolderId} not found in bucket {bucket.Name}") ;
        if (folder is not null)
        {
            var service = serviceFactory.GetFileStorageService(bucket.StorageAccount.Provider);

            var parentFolder = bucket.Folders.FirstOrDefault(f => f.Id == folder.ParentId);
            var prefix = (parentFolder?.GetFullPath() ?? "").TrimStart('/');
            var oldFolderPath = $"{prefix.TrimEnd('/')}/{folder.Name}/";
            var newFolderPath = $"{prefix.TrimEnd('/')}/{request.NewFolderName}/";
            // var fldrKey = $"{folder.GetFullPath().TrimStart($"/{bucket.Name}".ToArray())}/{request.NewFolderName}/.";   
            // await service.CreateEmptyFolderAsync(bucket.Name, fldrKey, bucket.StorageAccount.AccessKey, bucket.StorageAccount.SecretKey);
            await service.RenameFolderAsync(bucket.Name, oldFolderPath, newFolderPath, bucket.StorageAccount.AccessKey, bucket.StorageAccount.SecretKey);
            folder.ReName(request.NewFolderName);
            await repository.UpdateAsync(bucket);
        }
        return new RenameBucketFolderResponse(bucket.Id, request.FolderId, folder is null? "" :request.NewFolderName);
    }
}
