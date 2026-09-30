namespace FSH.Starter.WebApi.Document.Application.Buckets.RenameFile.v1;

public sealed record RenameBucketFileResponse(
    Guid BucketId,
    Guid FolderId,
    Guid FileId,
    string NewFileName);
