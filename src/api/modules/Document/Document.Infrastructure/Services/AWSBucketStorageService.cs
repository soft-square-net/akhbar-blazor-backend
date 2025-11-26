using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Storage.Bucket;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Starter.WebApi.Document.Infrastructure.Services.Auth;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FSH.Starter.WebApi.Document.Infrastructure.Services;
public class AWSBucketStorageService : IBucketStorageService
{
    private AmazonS3Client _s3Client;

    private IExternalRefreshingAWSWithBasicCredentials _refreshCeredintials;
    public AWSBucketStorageService(IExternalRefreshingAWSWithBasicCredentials refreshCeredintials, IConfiguration configuration)
    {
        AWSCredentials credentials = (AWSCredentials)refreshCeredintials;
        RegionEndpoint region = RegionEndpoint.GetBySystemName(configuration.GetValue<string>("AWS:Region")); // Specify your desired AWS region
        _refreshCeredintials = refreshCeredintials;
        _s3Client = new AmazonS3Client(credentials, region);
    }
    public async Task<CreateBucketResponse> CreateBucketAsync(CreateBucketCommand command)
    {
        //RegionEndpoint region = RegionEndpoint.GetBySystemName(command.Region);
        //AWSCredentials credentials = new BasicAWSCredentials(command.AccessKey, command.SecretKey);
        //using (var _s3Client = new AmazonS3Client(credentials, region))
        //{
        _refreshCeredintials.UpdateCredentials(command.AccessKey, command.SecretKey);
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, command.BucketName);
        if (bucketExists)
        {
            throw new NotFoundException($"Bucket with name {command.BucketName} already exists.");
        }
        else
        {
            var putBucketRequest = new PutBucketRequest
            {
                BucketName = command.BucketName,
                UseClientRegion = true
            };
            await _s3Client.PutBucketAsync(putBucketRequest);
            return new CreateBucketResponse(command.BucketName);

        }
        // }
            
    }

    public async Task<GetAllBucketsResponse> GetAllBucketsAsync(GetAllBucketsRequest request)
    {
        // RegionEndpoint region = RegionEndpoint.GetBySystemName(request.Region); // Specify your desired AWS region
        // AWSCredentials credentials = new BasicAWSCredentials(request.AccessKey, request.SecretKey);
        //using (var _s3Client = new AmazonS3Client(credentials, region))
        //{

        _refreshCeredintials.UpdateCredentials(request.AccessKey, request.SecretKey);
        var data = await _s3Client.ListBucketsAsync();
        return new GetAllBucketsResponse
        {
            BucketsNames = data.Buckets.Select(b => { return b.BucketName; }).ToList()
        };

        // }
    }

    public async Task DeleteBucketsAsync(DeleteBucketsRequest request)
    {
        //RegionEndpoint region = RegionEndpoint.GetBySystemName(request.Region);
        //AWSCredentials credentials = new BasicAWSCredentials(request.AccessKey, request.SecretKey);
        //using (var _s3Client = new AmazonS3Client(credentials, region))
        //{
        _refreshCeredintials.UpdateCredentials(request.AccessKey, request.SecretKey);
        await _s3Client.DeleteBucketAsync(request.BucketName);
        //}
    }
}
