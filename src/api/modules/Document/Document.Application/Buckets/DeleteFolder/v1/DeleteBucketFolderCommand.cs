using FSH.Framework.Core.Storage.File;
using MediatR;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.DeleteFolder.v1;
public sealed record DeleteBucketFolderCommand(
    Guid BucketId,
    Guid FolderId) : IRequest<DeleteBucketFolderResponse>;
