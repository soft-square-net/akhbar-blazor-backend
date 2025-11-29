using System.Net.Mime;
using MediatR;

namespace FSH.Framework.Core.Storage.Bucket.Features;

public class SvcCreateBucketCommand : IRequest<SvcCreateBucketResponse>
{
    public string BucketName { get; set; } = default!;
    public string Region { get; set; } = default!;
    public string AccessKey { get; set; } = default!;
    public string SecretKey { get; set; } = default!;
    public IDictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();

}

