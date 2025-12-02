using MediatR;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.DeleteFile.v1;
public sealed record DeleteBucketFileCommand(
    Guid BucketId,
    Guid FolderId,
    Guid FileId) : IRequest<DeleteBucketFileResponse>;
