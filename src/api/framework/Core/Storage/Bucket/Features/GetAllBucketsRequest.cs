using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace FSH.Framework.Core.Storage.Bucket.Features;
public class GetAllBucketsRequest: IRequest<GetAllBucketsResponse>
{
    public GetAllBucketsRequest(string region, string accessKey, string secretKey)
    {
        Region = region;
        AccessKey = accessKey;
        SecretKey = secretKey;
    }
    public string Region { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
}
