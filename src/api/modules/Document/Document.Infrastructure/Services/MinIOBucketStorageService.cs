using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Identity.Users.Abstractions;
using FSH.Framework.Core.Storage.Bucket;
using FSH.Framework.Core.Storage.Bucket.Features;
using FSH.Starter.WebApi.Document.Infrastructure.Services.Auth;
using Microsoft.Extensions.Configuration;
namespace FSH.Starter.WebApi.Document.Infrastructure.Services;
public class MinIOBucketStorageService : IBucketStorageService
{
    private readonly AmazonS3Client _s3Client;
    private readonly RegionEndpoint _region;
    private readonly ICurrentUser _user;

    private IExternalRefreshingAWSWithBasicCredentials _refreshCeredintials;
    public MinIOBucketStorageService(IExternalRefreshingAWSWithBasicCredentials refreshCeredintials, IConfiguration configuration, ICurrentUser user)
    {
        AWSCredentials credentials = (AWSCredentials)refreshCeredintials;
        _region = RegionEndpoint.GetBySystemName(configuration.GetValue<string>("AWS:Region")); // Specify your desired AWS region
        _refreshCeredintials = refreshCeredintials;
        _user = user;
        _s3Client = new AmazonS3Client(credentials, _region);
    }
    public async Task<SvcCreateBucketResponse> CreateBucketAsync(SvcCreateBucketCommand command)
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
                UseClientRegion = string.IsNullOrWhiteSpace(command.Region),
                PutBucketConfiguration = new PutBucketConfiguration
                {
                    Tags = new List<Tag> {
                             new Tag { Key = "CreatedById", Value = _user.GetUserId().ToString() },
                             new Tag { Key = "CreatedByEmail", Value = _user.GetUserEmail() },
                             new Tag { Key = "Tenant", Value = _user.GetTenant() },
                             new Tag { Key = "CreatedOn", Value = DateTime.UtcNow.ToString("o") }
                    },
                    LocationConstraint = string.IsNullOrWhiteSpace(command.Region)? _region.SystemName : command.Region
                    // _s3Client.Config.RegionEndpoint.SystemName,
                    // Location = new LocationInfo() { Name = _s3Client.Config.RegionEndpoint.SystemName, Type = new LocationType("your location type") }
                    // BucketInfo = new S3BucketInfo() { }
                }
                // BucketRegion = S3Region.USEast1,
                // BucketRegionName = _s3Client.Config.RegionEndpoint.SystemName,
                // CannedACL = S3CannedACL.Private,  // Not supported for directory buckets.
                // Grants = new List<S3Grant> {
                //     new S3Grant {
                //         Grantee = new S3Grantee {
                //             Type = GranteeType.Group,
                //             URI = "http://acs.amazonaws.com/groups/global/AllUsers"
                //         },
                //         Permission = S3Permission.READ
                //     }
                // },
                // ObjectLockEnabledForBucket = false, // Not supported for directory buckets.
                // ObjectOwnership = ObjectOwnership.ObjectWriter,

            };
            PutBucketResponse response = await _s3Client.PutBucketAsync(putBucketRequest);
            // response.BucketArn = $"arn:aws:s3:::{command.BucketName}";
            // response.Location = command.BucketName;
            // response.ResponseMetadata.Metadata;

            return new SvcCreateBucketResponse(command.BucketName, response.BucketArn,response.Location,response.ResponseMetadata.Metadata);
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
            HttpStatusCode = data.HttpStatusCode,
            MetaData = data.ResponseMetadata.Metadata,
            ContinuationToken = data.ContinuationToken,
            Prefix = data.Prefix,
            Buckets = data.Buckets.Select(b => { return new SingleBucketResponse(b.BucketName,b.BucketRegion,b.BucketArn,b.CreationDate); }).ToList()
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
