using System.Security.AccessControl;

namespace FSH.Starter.WebApi.Document.Application.Buckets.DownloadFile.v1;

public sealed record DownloadBucketFileResponse(
    Stream downloadedStream ,
    string contentType,
    string contentLanguage,
    string contentDisposition,
    string contentMD5,
    long contentLength,
    DateTime? expires = null
    );
