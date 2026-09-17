using FSH.Starter.WebApi.Document.Domain;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.GetFile.v1;
public sealed record GetBucketFileResponse(Guid BucketId,
    Guid FileId,
    Folder Folder,
    string Key,
    string Name,
    string Description,
    string Extension,
    string Etag,
    string Url,
    FileType FileType = FileType.Other,
    long? Size = 0,
    bool IsPublic = true);

