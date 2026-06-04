using FluentValidation;
using FSH.Starter.WebApi.Document.Application.AccessRules.Update.v1;

namespace FSH.Starter.WebApi.Document.Application.AccessRules.Update.v1;
public class UpdateAccessRuleCommandValidator : AbstractValidator<UpdateAccessRuleCommand>
{
    public UpdateAccessRuleCommandValidator()
    {
        RuleFor(b => b.Id).NotEmpty();
        RuleFor(b => b.Read).Equals(true);
        RuleFor(b => b.Write).NotNull();
        RuleFor(b => b.Execute).NotNull();
        RuleFor(b => b.IsEnabled).NotNull();
        RuleFor(b => b.Description).MaximumLength(1000);
    }
}
