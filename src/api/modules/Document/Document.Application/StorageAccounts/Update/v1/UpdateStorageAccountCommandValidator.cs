using FluentValidation;
using FSH.Starter.WebApi.Document.Application.StorageAccounts.Update.v1;

namespace FSH.Starter.WebApi.Document.Application.Documents.Update.v1;
public class UpdateStorageAccountCommandValidator : AbstractValidator<UpdateStorageAccountCommand>
{
    public UpdateStorageAccountCommandValidator()
    {
        RuleFor(b => b.AccountName).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.Description).MaximumLength(1000);
    }
}
