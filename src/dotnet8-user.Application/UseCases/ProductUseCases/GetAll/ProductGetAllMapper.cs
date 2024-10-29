using AutoMapper;
using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.UseCases.ProductUseCases.GetAll
{
    public sealed class ProductGetAllMapper : Profile
    {
        public ProductGetAllMapper()
        {

            CreateMap<Products, ProductGetAllResponse>();
            CreateMap<Products, ProductShortResponse>();
        }
    }
}
