namespace FSH.Starter.WebApi.Document.Application.Buckets.RenameFolder.v1;

public sealed record  RenameBucketFolderResponse(
    Guid BucketId,
    Guid FolderId,
    string NewFolderName);
