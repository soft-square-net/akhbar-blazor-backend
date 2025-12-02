using FSH.Framework.Core.Storage.File;
using FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
using MediatR;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.GetFile.v1;
public sealed class GetBucketFileRequest : IRequest<GetBucketFileResponse>
{
    public Guid BucketId { get; set; }
    public Guid FolderId { get; set; }
    public Guid FileId { get; set; }
    public GetBucketFileRequest(Guid bucketId, Guid folderId, Guid fileId)
    {
        BucketId = bucketId;
        FolderId = folderId;
        FileId = fileId;
    }
}

public class GetBucketRequest : IRequest<BucketResponse>
{
    public Guid Id { get; set; }
    public GetBucketRequest(Guid id) => Id = id;
}
