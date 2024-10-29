using AutoMapper;
using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.UseCases.ProductUseCases.Deactivate
{
    public sealed class ProductDeactivateMapper : Profile
    {
        public ProductDeactivateMapper()
        {
            CreateMap<ProductDeactivateRequest, Products>();
            CreateMap<Products, ProductResponse>();
        }
    }
}
