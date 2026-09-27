using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Starter.WebApi.Document.Application.Buckets.Specs;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Folder = FSH.Starter.WebApi.Document.Domain.Folder;

namespace FSH.Starter.WebApi.Document.Application.Buckets.GetFolder.v1;
public sealed class GetBucketFolderHandler(
    ILogger<GetBucketFolderHandler> logger, IStorageServiceFactory serviceFactory, 
    [FromKeyedServices("document:buckets")] IRepository<Bucket> repository,
    [FromKeyedServices("document:folders")] IReadRepository<Folder> folderRepository
    ) : IRequestHandler<GetBucketFolderRequest, GetBucketFolderResponse>
{
   
    public async Task<GetBucketFolderResponse?> Handle(GetBucketFolderRequest request, CancellationToken cancellationToken)
    {
        var bucket = await repository.FirstOrDefaultAsync(new GetBucketByIdSpec(request.BucketId), cancellationToken);
        GetBucketFolderResponse result;
        // var service = serviceFactory.GetFileStorageService(StorageProvider.AmazonS3);
        if (request.FolderId.HasValue && request.FolderId != Guid.Empty) 
        {
            var folder = await folderRepository.GetByIdAsync(request.FolderId.Value, cancellationToken);
            if (folder is null)
            {
                logger.LogWarning("Folder with id {FolderId} not found in bucket {BucketId}", request.FolderId, request.BucketId);
                return null;
            }
            result = new GetBucketFolderResponse(folder.Id, bucket.Id, folder.Name, folder.Children, folder.Files, folder.Files.Count, folder.IsRoot, folder.Created, folder.LastModified, true, true, true);
            return result;
        }
        else
        {
            foreach (var item in bucket.Folders.Where(f => f.IsRoot || f.Name == "/"))
            {
                result = new GetBucketFolderResponse(item.Id, bucket.Id, bucket.Name, item.Children, item.Files, item.Files.Count, item.IsRoot, item.Created, item.LastModified, true, true, true);
                return result;
            }
        }
        
        return null;
    }
}
