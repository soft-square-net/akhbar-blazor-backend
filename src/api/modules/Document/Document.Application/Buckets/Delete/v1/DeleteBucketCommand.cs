using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Documents.Delete.v1;
public sealed record DeleteBucketCommand(
    Guid Id) : IRequest;
