using dotnet8_user.Application.UseCases.UserUseCases.Create;
using FluentValidation;

namespace dotnet8_user.Application.UseCases.UserUseCases.Common
{
    public sealed class UserValidator : AbstractValidator<UserCreateRequest>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
            RuleFor(x => x.FullName).NotEmpty().MinimumLength(3).MaximumLength(50);
        }
    }
}
