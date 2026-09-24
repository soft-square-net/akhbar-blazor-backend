using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Buckets.DeleteFile.v1;
public sealed record DeleteBucketFileCommand(
    Guid BucketId,
    Guid FolderId,
    Guid FileId) : IRequest<DeleteBucketFileResponse>;
