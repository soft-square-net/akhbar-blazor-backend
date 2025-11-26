using System.Net.Mime;
using MediatR;

namespace FSH.Framework.Core.Storage.Bucket.Features;

public class CreateBucketCommand : IRequest<CreateBucketResponse>
{
    public string BucketName { get; set; } = default!;
    public string Region { get; set; } = default!;
    public string AccessKey { get; set; } = default!;
    public string SecretKey { get; set; } = default!;

}

