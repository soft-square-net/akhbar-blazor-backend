using MediatR;

namespace FSH.Starter.WebApi.Document.Application.AccessRules.Delete.v1;
public sealed record DeleteAccessRuleCommand(
    Guid Id) : IRequest;
