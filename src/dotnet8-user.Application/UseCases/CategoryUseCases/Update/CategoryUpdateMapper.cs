using AutoMapper;
using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.Update
{
    public sealed class CategoryUpdateMapper : Profile
    {
        public CategoryUpdateMapper()
        {
            CreateMap<CategoryUpdateRequest, Category>();
            CreateMap<Category, CategoryResponse>();
        }
    }
}
