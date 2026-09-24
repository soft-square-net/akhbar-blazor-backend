using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Buckets.DownloadFile.v1;

public sealed record DownloadBucketFileRequest(Guid BucketId, Guid FileId) : IRequest<DownloadBucketFileResponse>;
