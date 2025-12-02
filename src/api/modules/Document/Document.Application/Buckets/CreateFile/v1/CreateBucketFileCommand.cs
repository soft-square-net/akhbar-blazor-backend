using FSH.Framework.Core.Storage.File;
using MediatR;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.CreateFile.v1;
public sealed record CreateBucketFileCommand(
    Guid BucketId,
    Guid ParentFolderId,
    string FileName,
    string Description,
    string FileExtension,
    string ContentType,
    FileType FileType,
    Stream FileContent) : IRequest<CreateBucketFileResponse>;
