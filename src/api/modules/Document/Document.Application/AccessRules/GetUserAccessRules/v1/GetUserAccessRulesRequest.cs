
using FSH.Starter.WebApi.Document.Application.AccessRules.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.Document.Appication.AccessRules.GetUserAccessRules.v1;

public sealed record GetUserAccessRulesRequest(Guid? UserId) : IRequest<List<GetUserAccessRulesResponse>>;
//{
//    public Guid UserId { get; set; }
//    public GetUserAccessRulesRequest(Guid userId) => UserId = userId;
//}
