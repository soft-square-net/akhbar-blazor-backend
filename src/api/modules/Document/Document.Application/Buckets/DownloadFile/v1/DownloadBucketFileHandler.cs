using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Starter.WebApi.Document.Application.Buckets.Specs;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Application.Buckets.DownloadFile.v1;
public sealed class DownloadBucketFileHandler(
    ILogger<DownloadBucketFileHandler> logger, IStorageServiceFactory serviceFactory, 
    [FromKeyedServices("document:buckets")] IRepository<Bucket> repository
    ) : IRequestHandler<DownloadBucketFileRequest, DownloadBucketFileResponse>
{
   
    public async Task<DownloadBucketFileResponse> Handle(DownloadBucketFileRequest request, CancellationToken cancellationToken)
    {
        // var bucket = await repository.SingleOrDefaultAsync(new BucketNavigateToFoldersSpec(request.BucketId), cancellationToken);

        Stream stream = new MemoryStream();
       
        return new DownloadBucketFileResponse(stream);
    }
}
