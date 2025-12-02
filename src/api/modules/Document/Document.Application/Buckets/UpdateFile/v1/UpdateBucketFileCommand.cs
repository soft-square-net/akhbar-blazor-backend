using FSH.Framework.Core.Storage.File;
using MediatR;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.UpdateFile.v1;
public sealed record UpdateBucketFileCommand(
    Guid BucketId,
    Guid FileId) : IRequest<UpdateBucketFileResponse>;
