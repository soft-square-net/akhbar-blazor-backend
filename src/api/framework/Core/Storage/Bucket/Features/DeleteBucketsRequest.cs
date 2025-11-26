using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace FSH.Framework.Core.Storage.Bucket.Features;
public class DeleteBucketsRequest : IRequest<GetAllBucketsResponse>
{
    public DeleteBucketsRequest(string region, string accessKey, string secretKey, string bucketName)
    {
        Region = region;
        AccessKey = accessKey;
        SecretKey = secretKey;
        BucketName = bucketName;
    }
    public string Region { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string BucketName { get; set; }
}
