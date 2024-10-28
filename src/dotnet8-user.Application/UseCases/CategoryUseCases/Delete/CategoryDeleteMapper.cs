using AutoMapper;
using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.Delete
{
    public sealed class CategoryDeleteMapper : Profile
    {
        public CategoryDeleteMapper()
        {
            CreateMap<CategoryDeleteRequest, Category>();
            CreateMap<Category, CategoryResponse>();
        }
    }
}
