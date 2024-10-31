using dotnet8_user.Application.Interfaces;
using FluentValidation;

namespace dotnet8_user.Application.UseCases.ProductUseCases.Common
{
    public sealed class ProductValidator<T> : AbstractValidator<T> where T : IProductRequest
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.StockQuantity).NotEmpty();
            RuleFor(x => x.Brand).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.ImageUrl).NotEmpty().MinimumLength(3);
        }
    }
}
