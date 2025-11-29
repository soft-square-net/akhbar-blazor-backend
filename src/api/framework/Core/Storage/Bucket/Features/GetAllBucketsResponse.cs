using System.Collections.Generic;
using System.Net;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FSH.Framework.Core.Storage.Bucket.Features;

public record SingleBucketResponse(string Name,string Region, string ResourceName, DateTime? Created);
public class GetAllBucketsResponse
{
    public IDictionary<string,string> MetaData { get; set; } = default!;
    public HttpStatusCode HttpStatusCode { get; set; }
    public string ContinuationToken { get; set; } = string.Empty;
    public string Prefix { get; set; } = string.Empty;
    public List<SingleBucketResponse> Buckets { get; set; } = default!;
}

