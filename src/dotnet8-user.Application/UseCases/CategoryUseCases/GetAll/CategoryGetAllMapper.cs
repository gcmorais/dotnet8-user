using AutoMapper;
using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.GetAll
{
    public sealed class CategoryGetAllMapper : Profile
    {
        public CategoryGetAllMapper()
        {
            CreateMap<CategoryGetAllRequest, Category>();
            CreateMap<Category, CategoryResponse>();
        }
    }
}
