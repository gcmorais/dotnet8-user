using AutoMapper;
using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.UseCases.ProductUseCases.Update
{
    public sealed class ProductUpdateMapper : Profile
    {
        public ProductUpdateMapper()
        {
            CreateMap<ProductUpdateRequest, Products>();
            CreateMap<Products, ProductResponse>();
        }
    }
}
