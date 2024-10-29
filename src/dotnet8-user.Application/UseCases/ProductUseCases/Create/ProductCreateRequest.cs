using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using MediatR;

namespace dotnet8_user.Application.UseCases.ProductUseCases.Create
{
    public sealed record ProductCreateRequest : IRequest<ProductResponse>, IProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Brand { get; set; }
        public string ImageUrl { get; set; }
        public Guid CategoryId { get; set; }
    }
}
