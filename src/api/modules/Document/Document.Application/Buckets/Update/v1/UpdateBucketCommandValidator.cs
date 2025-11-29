using FluentValidation;

namespace FSH.Starter.WebApi.Document.Application.Buckets.Update.v1;
public class UpdateBucketCommandValidator : AbstractValidator<UpdateBucketCommand>
{
    public UpdateBucketCommandValidator()
    {
        RuleFor(b => b.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.Description).MaximumLength(1000);
    }
}
