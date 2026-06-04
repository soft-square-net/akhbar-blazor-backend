using MediatR;

namespace FSH.Starter.WebApi.Document.Application.AccessRules.Get.v1;
public class GetAccessRuleRequest : IRequest<AccessRuleResponse>
{
    public Guid Id { get; set; }
    public GetAccessRuleRequest(Guid id) => Id = id;
}
