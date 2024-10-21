using dotnet_user_api.Application.UseCases.CategoryUseCases.Create;
using FluentValidation;

namespace dotnet_user_api.Application.UseCases.CategoryUseCases.Common
{
    public sealed class CategoryValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(20);
        }
    }
}
