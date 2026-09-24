using FSH.Framework.Core.Storage.File;
using MediatR;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Application.Buckets.UpdateFolder.v1;
public sealed record UpdateBucketFolderCommand(
    Guid BucketId,
    Guid FolderId) : IRequest<UpdateBucketFolderResponse>;
