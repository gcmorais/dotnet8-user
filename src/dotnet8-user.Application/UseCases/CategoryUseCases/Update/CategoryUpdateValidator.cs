using FluentValidation;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.Update
{
    public sealed class CategoryUpdateValidator : AbstractValidator<CategoryUpdateRequest>
    {
        public CategoryUpdateValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty().MinimumLength(3).MaximumLength(50);
        }
    }
}
