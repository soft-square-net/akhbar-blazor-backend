using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Amazon.S3;
using Amazon.S3.Model;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Storage.Dtos;
using FSH.Framework.Core.Storage.File;
using FSH.Framework.Core.Storage.File.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FSH.Starter.WebApi.Document.Infrastructure.Services;
public class AWSFileStorageService : IFileStorageService
{
    private readonly IAmazonS3 _s3Client;

    public AWSFileStorageService(IAmazonS3 amazonS3)
    {
        _s3Client = amazonS3;
    }

    public async Task<IEnumerable<S3ObjectDto>> GetAllFilesAsync<T>(string bucketName, string? prefix, CancellationToken cancellationToken = default)
    {
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
        if (!bucketExists) throw new NotFoundException($"Bucket {bucketName} does not exist.");
        var request = new ListObjectsV2Request()
        {
            BucketName = bucketName,
            Prefix = prefix
        };
        var result = await _s3Client.ListObjectsV2Async(request);
        var s3Objects = result.S3Objects.Select(s =>
        {
            var urlRequest = new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Key = s.Key,
                Expires = DateTime.UtcNow.AddMinutes(1)
            };
            return new S3ObjectDto()
            {
                Name = s.Key.ToString(),
                PresignedUrl = _s3Client.GetPreSignedURL(urlRequest),
            };
        });
        return s3Objects;
    }

    /// <summary>
    /// Get the File PresignedUrl
    /// </summary>
    /// <param name="bucketName"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<S3ObjectDto> GetFileByKeyAsync(string bucketName, string key, CancellationToken cancellationToken = default)
    {
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
        if (!bucketExists) throw new NotFoundException($"Bucket {bucketName} does not exist.");
        var s3Object = await _s3Client.GetObjectAsync(bucketName, key);
        // return new Stream (s3Object.ResponseStream, s3Object.Headers.ContentType);
        var urlRequest = new GetPreSignedUrlRequest()
        {
            BucketName = bucketName,
            Key = s3Object.Key,
            Expires = DateTime.UtcNow.AddMinutes(1)
        };
        return new S3ObjectDto()
        {
            Name = s3Object.Key.ToString(),
            PresignedUrl = _s3Client.GetPreSignedURL(urlRequest),
        };
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string bucketName, string fileName, string contentType, FileType fileType, string fileExtention, string? prefix, CancellationToken cancellationToken = default)
    {
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
        if (!bucketExists) throw new FileNotFoundException($"Bucket {bucketName} does not exist.");
        var request = new PutObjectRequest()
        {
            BucketName = bucketName,
            Key = string.IsNullOrEmpty(prefix) ? fileName : $"{prefix?.TrimEnd('/')}/{fileName}",
            InputStream = fileStream
        };
        request.Metadata.Add("Content-Type", contentType);
        await _s3Client.PutObjectAsync(request);
        return $"File {prefix}/{fileName} uploaded to S3 successfully!";
    }

    public async Task<FileDpwmloadResponse> DownloadFileAsync(string bucketName, string key, CancellationToken cancellationToken = default)
    {
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
        if (!bucketExists) throw new NotFoundException($"Bucket {bucketName} does not exist.");
        var s3Object = await _s3Client.GetObjectAsync(bucketName, key);
        // return new Stream (s3Object.ResponseStream, s3Object.Headers.ContentType);
        return new FileDpwmloadResponse(
            s3Object.ResponseStream,
            s3Object.Headers.ContentType,
            s3Object.Headers.ContentLength,
            s3Object.Headers.ContentLanguage,
            s3Object.Headers.ContentDisposition,
            s3Object.Headers.ContentEncoding,
            s3Object.Headers.ContentMD5,
            s3Object.Headers.Expires
            );
    }

    public async Task DeleteFileAsync(string bucketName, string key, CancellationToken cancellationToken = default)
    {
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
        if (!bucketExists) throw new NotFoundException($"Bucket {bucketName} does not exist");
        await _s3Client.DeleteObjectAsync(bucketName, key);
        // return NoContent();
    }

    

    

    
}
