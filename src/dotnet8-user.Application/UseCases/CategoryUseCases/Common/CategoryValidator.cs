using FluentValidation;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.Common
{
    public sealed class CategoryValidator<T> : AbstractValidator<T> where T : ICategoryRequest
    {
        public CategoryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty().MinimumLength(3).MaximumLength(50);
        }
    }
}
