using AutoMapper;
using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.Create
{
    public sealed class CategoryCreateMapper : Profile
    {
        public CategoryCreateMapper()
        {
            CreateMap<CategoryCreateRequest, Category>();
            CreateMap<Category, CategoryResponse>();
        }
    }
}
