using MediatR;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.UploadFile.v1;

public sealed record UploadBucketFileRequest(Guid BucketId, Guid FolderId, Guid FileId) : IRequest<UploadBucketFileResponse>;
