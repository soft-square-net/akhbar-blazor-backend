
using MediatR;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.CreateFolder.v1;
public sealed record CreateBucketFolderCommand(
    Guid BucketId,
    Guid ParentFolderId,
    string FolderName,
    string Description):IRequest<CreateBucketFolderResponse>;
