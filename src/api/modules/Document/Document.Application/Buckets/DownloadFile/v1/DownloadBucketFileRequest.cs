using MediatR;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.DownloadFile.v1;

public sealed record DownloadBucketFileRequest(Guid BucketId, Guid FolderId, Guid FileId) : IRequest<DownloadBucketFileResponse>;
