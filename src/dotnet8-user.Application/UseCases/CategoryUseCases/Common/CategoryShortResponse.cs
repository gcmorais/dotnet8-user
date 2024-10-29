using dotnet8_user.Application.UseCases.ProductUseCases.Common;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.Common
{
    public sealed record CategoryShortResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ProductShortResponse> Products { get; set; }
    }
}
