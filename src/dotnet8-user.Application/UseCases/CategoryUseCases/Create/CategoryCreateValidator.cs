using FluentValidation;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.Create
{
    public sealed class CategoryCreateValidator : AbstractValidator<CategoryCreateRequest>
    {
        public CategoryCreateValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty().MinimumLength(3).MaximumLength(50);
        }
    }
}
