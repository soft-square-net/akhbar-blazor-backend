using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Buckets.Update.v1;
public sealed record UpdateBucketCommand(
    Guid Id,
    string? Name,
    string? Description = null) : IRequest<UpdateBucketResponse>;
