using FluentValidation;

namespace dotnet8_user.Application.UseCases.UserUseCases.Update
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("A valid email is required.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .When(x => !string.IsNullOrWhiteSpace(x.UserName));

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName is required.")
                .When(x => !string.IsNullOrWhiteSpace(x.FullName));
        }
    }
}
