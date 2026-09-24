using FSH.Starter.WebApi.Document.Domain;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Application.Buckets.GetBucketFile.v1;
public record GetBucketFileResponse(
    Guid BucketId,
    Guid FileId,
    Folder Folder,
    string Key,
    string Name,
    string Description,
    string Extension,
    string Etag,
    string Url,
    DateTimeOffset Created,
    DateTimeOffset LastModified,
    FileType FileType = FileType.Other,
    long? Size = 0,
    bool IsPublic = true);

