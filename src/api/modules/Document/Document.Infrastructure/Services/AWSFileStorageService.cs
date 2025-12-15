using System;
using System.Text.RegularExpressions;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Storage.Dtos;
using FSH.Framework.Core.Storage.File;
using FSH.Framework.Core.Storage.File.Features;
using FSH.Starter.WebApi.Document.Infrastructure.Services.Auth;
using Microsoft.Extensions.Configuration;

namespace FSH.Starter.WebApi.Document.Infrastructure.Services;
public class AWSFileStorageService : IFileStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly IExternalRefreshingAWSWithBasicCredentials _refreshCeredintials;
    private string _accessKey = string.Empty;
    private string _secretKey = string.Empty;
    public AWSFileStorageService(IExternalRefreshingAWSWithBasicCredentials refreshCeredintials, IConfiguration configuration)
    {
        AWSCredentials credentials = (AWSCredentials)refreshCeredintials;
        RegionEndpoint region = RegionEndpoint.GetBySystemName(configuration.GetValue<string>("AWS:Region")); // Specify your desired AWS region
        _refreshCeredintials = refreshCeredintials;
        _s3Client = new AmazonS3Client(credentials, region);
    }

    public bool UpdateCredentials(string accessKey, string secretKey)
    {
        _accessKey = accessKey;
        _secretKey = secretKey;
        return _refreshCeredintials.UpdateCredentials(accessKey, secretKey);
    }
    public async Task<IEnumerable<S3ObjectDto>> GetAllFilesAsync<T>(string bucketName, string? prefix, string accessKey, string secretKey, CancellationToken cancellationToken = default)
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
    public async Task<S3ObjectDto> GetFileByKeyAsync(string bucketName, string key, string accessKey, string secretKey, CancellationToken cancellationToken = default)
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

    public async Task<string> UploadFileAsync(Stream fileStream, string bucketName, string fileName, string contentType, FileType fileType, string fileExtention, string? prefix,string accessKey, string secretKey, CancellationToken cancellationToken = default)
    {
        _refreshCeredintials.UpdateCredentials(accessKey, secretKey);
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
        if (!bucketExists) throw new FileNotFoundException($"Bucket {bucketName} does not exist.");
        
        var request = new PutObjectRequest()
        {
            BucketName = bucketName,
            Key = string.IsNullOrEmpty(prefix) ? fileName : $"{prefix?.TrimStart('/').TrimEnd('/')}/{fileName}",
            InputStream = fileStream
        };
        request.Metadata.Add("Content-Type", contentType);
        PutObjectResponse res = await _s3Client.PutObjectAsync(request, cancellationToken);
        var eTag = res.ETag;
        var expires = res.Expiration;
        _refreshCeredintials.UpdateCredentials("", "");
        return request.Key;
    }

    public async Task<string> GetPreSingedUrlAsync(string bucketName, string key, string accessKey, string secretKey, CancellationToken cancellationToken = default)
    {
        _refreshCeredintials.UpdateCredentials(accessKey, secretKey);
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
        if (!bucketExists) throw new NotFoundException($"Bucket {bucketName} does not exist.");

        
        var urlRequest = new GetPreSignedUrlRequest()
        {
            BucketName = bucketName,
            Key = key,
            Expires = DateTime.UtcNow.AddMinutes(1),
            Verb = HttpVerb.GET
        };
        _refreshCeredintials.UpdateCredentials("", "");
        return await _s3Client.GetPreSignedURLAsync(urlRequest);
    }

    public async Task<Uri> UploadFileAsync<T>(FileUploadCommand? command, FileType supportedFileType, string accessKey, string secretKey, CancellationToken cancellationToken = default) where T : class
    {
        
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, command.Bucket);
        if (!bucketExists) throw new FileNotFoundException($"Bucket {command.Bucket} does not exist.");

        string base64Data = Regex.Match(command.Data, "data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

        var streamData = new MemoryStream(Convert.FromBase64String(base64Data));
        var request = new PutObjectRequest()
        {
            BucketName = command.Bucket,
            Key = string.IsNullOrEmpty(command.Prefix) ? command.Name : $"{command.Prefix?.TrimEnd('/')}/{command.Name}",
            InputStream = streamData
        };
        request.Metadata.Add("Content-Type", command.ContentType);
        await _s3Client.PutObjectAsync(request);

        var urlRequest = new GetPreSignedUrlRequest()
        {
            BucketName = request.BucketName,
            Key = request.Key,
            Expires = DateTime.UtcNow.AddMinutes(1),
            ContentType = request.ContentType,
            Verb = HttpVerb.GET
        };

        return new Uri(await _s3Client.GetPreSignedURLAsync(urlRequest));
        //return new S3ObjectDto()
        //{
        //    Name = request.Key.ToString(),
        //    PresignedUrl = _s3Client.GetPreSignedURL(urlRequest)
        //};
        // return $"File {command.Prefix}/{command.Name} uploaded to S3 successfully!";
        // return $"File {command.Prefix}/{command.Name} uploaded to S3 successfully!";
    }

    public async Task<FileDownloadResponse> DownloadFileAsync(string bucketName, string key, string accessKey, string secretKey, CancellationToken cancellationToken = default)
    {
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
        if (!bucketExists) throw new NotFoundException($"Bucket {bucketName} does not exist.");
        var s3Object = await _s3Client.GetObjectAsync(bucketName, key);
        // return new Stream (s3Object.ResponseStream, s3Object.Headers.ContentType);
        return new FileDownloadResponse(
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

    public async Task DeleteFileAsync(string bucketName, string key, string accessKey, string secretKey, CancellationToken cancellationToken = default)
    {
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
        if (!bucketExists) throw new NotFoundException($"Bucket {bucketName} does not exist");
        await _s3Client.DeleteObjectAsync(bucketName, key);
        // return NoContent();
    }



    public async Task CreateEmptyFolderAsync(string bucketName, string folderName, string accessKey, string secretKey)
    {
        var putObjectRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = folderName, // Key ending with a slash
            ContentBody = string.Empty // Empty content for a zero-byte object
        };
        await _s3Client.PutObjectAsync(putObjectRequest);
    }



    public async Task UploadFileToFolderAsync(string bucketName, string fileKeyInS3, string localFilePath, string accessKey, string secretKey)
    {
        using (var fileStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read))
        {
            var putObjectRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = fileKeyInS3,
                InputStream = fileStream
            };
            await _s3Client.PutObjectAsync(putObjectRequest);
        }
    }
}
