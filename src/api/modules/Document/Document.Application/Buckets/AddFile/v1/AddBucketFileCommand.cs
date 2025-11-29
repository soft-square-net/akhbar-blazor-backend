using FSH.Framework.Core.Storage.File;
using MediatR;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.AddFile.v1;
public sealed record AddBucketFileCommand(
    Guid BucketId,
    Guid ParentFolderId,
    string FileName,
    string Description,
    string FileExtension,
    string ContentType,
    FileType FileType,
    Stream FileContent) : IRequest<AddBucketFileResponse>;
