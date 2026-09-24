using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.WebApi.Document.Application.Buckets.GetBucketFile.v1;
using FSH.Starter.WebApi.Document.Domain;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Application.Buckets.SearchFiles.v1;

public sealed record SearchBucketFileResponse : GetBucketFileResponse
{
    public SearchBucketFileResponse(Guid BucketId, Guid FileId, Folder Folder, string Key, string Name, string Description, string Extension, string Etag, string Url, DateTimeOffset Created, DateTimeOffset LastModified, FileType FileType = FileType.Other, long? Size = 0, bool IsPublic = true) : base(BucketId, FileId, Folder, Key, Name, Description, Extension, Etag, Url, Created, LastModified, FileType, Size, IsPublic)
    {
    }
}
