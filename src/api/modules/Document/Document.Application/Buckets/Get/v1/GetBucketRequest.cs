using FSH.Framework.Core.Storage.Bucket.Features;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
public class GetBucketRequest : IRequest<BucketResponse>
{
    public Guid Id { get; set; }
    public GetBucketRequest(Guid id) => Id = id;
}
