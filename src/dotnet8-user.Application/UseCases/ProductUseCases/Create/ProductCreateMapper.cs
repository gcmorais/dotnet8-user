using AutoMapper;
using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.UseCases.ProductUseCases.Create
{
    public sealed class ProductCreateMapper : Profile
    {
        public ProductCreateMapper()
        {
            CreateMap<ProductCreateRequest, Products>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
            CreateMap<Products, ProductResponse>();
        }
    }
}
