using FSH.Framework.Core.Storage.File;
using MediatR;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.CreateFile.v1;
public sealed record CreateBucketFileCommand(
    Guid BucketId,
    Guid ParentFolderId,
    FileType FileType,
    string FileName,
    //string Description,
    //string FileExtension,
    string ContentType,
    long FileSize,
    Stream FileContent) : IRequest<CreateBucketFileResponse>;
