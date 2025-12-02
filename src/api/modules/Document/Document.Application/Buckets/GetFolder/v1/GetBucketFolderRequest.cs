using FSH.Framework.Core.Storage.File;
using FSH.Starter.WebApi.Document.Appication.Buckets.GetFolder.v1;
using MediatR;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.GetFolder.v1;
public sealed record GetBucketFolderRequest : IRequest<GetBucketFolderResponse> {
    public GetBucketFolderRequest(Guid bucketId, Guid folderId)
    {
        BucketId = bucketId;
        FolderId = folderId;
    }
    public Guid BucketId { get; set; }
    public Guid FolderId { get; set; }
}
