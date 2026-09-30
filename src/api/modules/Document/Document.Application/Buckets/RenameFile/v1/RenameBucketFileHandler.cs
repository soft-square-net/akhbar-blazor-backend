using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using FSH.Starter.WebApi.Document.Application.Buckets.RenameFile.v1;
using FSH.Starter.WebApi.Document.Application.Buckets.Specs;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Enums;
using Folder = FSH.Starter.WebApi.Document.Domain.Folder;
using File = FSH.Starter.WebApi.Document.Domain.File;

namespace FSH.Starter.WebApi.Document.Application.Buckets.CreateFolder.v1;
public sealed class RenameBucketFileHandler(
        ILogger<RenameBucketFileHandler> logger,
        IStorageServiceFactory serviceFactory,
        [FromKeyedServices("document:buckets")] IRepository<Bucket> repository
    ) : IRequestHandler<RenameBucketFileCommand,RenameBucketFileResponse> {


    public async Task<RenameBucketFileResponse> Handle(RenameBucketFileCommand request, CancellationToken cancellationToken)
    {
        var bucket = await repository.FirstOrDefaultAsync(new GetBucketByIdSpec(request.BucketId));
        Folder folder = bucket.Folders.FirstOrDefault(f => f.Id == request.FolderId) ?? throw new Exception($"Folder with id {request.FolderId} not found in bucket {bucket.Name}");
        var service = serviceFactory.GetFileStorageService(bucket.StorageAccount.Provider);

        var fldrKey = $"{folder.GetFullPath().TrimStart($"/{bucket.Name}".ToArray())}/{request.NewFileName}/.";   
        // await service.CreateEmptyFolderAsync(bucket.Name, fldrKey, bucket.StorageAccount.AccessKey, bucket.StorageAccount.SecretKey);

        await repository.UpdateAsync(bucket);
        return new RenameBucketFileResponse(bucket.Id, folder.Id, request.FileId,request.NewFileName);
    }
}
