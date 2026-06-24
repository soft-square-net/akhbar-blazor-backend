using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Buckets.Update.v1;
public sealed record UpdateBucketCommand(
    Guid Id,
    Guid? StorageAccountId,
    string? Name,
    string? Description = null) : IRequest<UpdateBucketResponse>;
