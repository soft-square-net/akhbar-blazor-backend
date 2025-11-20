using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Framework.Core.Storage.Dtos;
using FSH.Framework.Core.Storage.File.Features;

namespace FSH.Framework.Core.Storage.File;
public interface IFileStorageService
{
    public Task<Uri> UploadFileAsync<T>(FileUploadCommand? request, FileType supportedFileType, CancellationToken cancellationToken = default)
    where T : class;
    Task<string> UploadFileAsync(Stream fileStream, string bucketName, string fileName, string contentType, FileType fileType, string fileExtention, string? prefix, CancellationToken cancellationToken = default);
    Task<FileDpwmloadResponse> DownloadFileAsync(string bucketName, string key, CancellationToken cancellationToken = default);
    Task<IEnumerable<S3ObjectDto>> GetAllFilesAsync<T>(string bucketName, string? prefix, CancellationToken cancellationToken = default);
    Task<S3ObjectDto> GetFileByKeyAsync(string bucketName, string key, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(string bucketName, string key, CancellationToken cancellationToken = default);
}

/// <param name="fullPath">Blob metadata</param>
/// <param name="dataStream">Stream to upload from</param>
/// <param name="cancellationToken"></param>
/// <param name="append">When true, appends to the file instead of writing a new one.</param>
/// <returns>Writeable stream</returns>
/// <exception cref="ArgumentNullException">Thrown when any parameter is null</exception>
/// <exception cref="ArgumentException">Thrown when ID is too long. Long IDs are the ones longer than 50 characters.</exception>
/// Task WriteAsync(string fullPath, Stream dataStream, bool append = false, CancellationToken cancellationToken = default);

