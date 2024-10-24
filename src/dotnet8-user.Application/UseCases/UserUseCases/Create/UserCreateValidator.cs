using FluentValidation;

namespace dotnet8_user.Application.UseCases.UserUseCases.Create
{
    public sealed class UserCreateValidator : AbstractValidator<UserCreateRequest>
    {
        public UserCreateValidator()
        {
            RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
            RuleFor(x => x.FullName).NotEmpty().MinimumLength(3).MaximumLength(50);
        }
    }
}
