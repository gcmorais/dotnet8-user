using AutoMapper;
using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.UseCases.ProductUseCases.Delete
{
    public sealed class ProductDeleteMapper : Profile
    {
        public ProductDeleteMapper()
        {
            CreateMap<ProductDeleteRequest, Products>();
            CreateMap<Products, ProductResponse>();
        }
    }
}
