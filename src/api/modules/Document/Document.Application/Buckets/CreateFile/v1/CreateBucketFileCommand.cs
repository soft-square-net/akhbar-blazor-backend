
using MediatR;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Application.Buckets.CreateFile.v1;
public sealed record CreateBucketFileCommand(
    Guid BucketId,
    Guid ParentFolderId,
    FileType FileType,
    string FileName,
    string Description,
    string FileExtension,
    string ContentType,
    long FileSize,
    Stream FileContent,
    Dictionary<string,string> metadata) : IRequest<CreateBucketFileResponse>;
