using AutoMapper;
using dotnet_user_api.Application.UseCases.CategoryUseCases.Common;
using dotnet_user_api.Domain.Entities;

namespace dotnet_user_api.Application.UseCases.CategoryUseCases.Create
{
    public sealed class CreateCategoryMapper : Profile
    {
        public CreateCategoryMapper()
        {
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<Category, CategoryResponse>();
        }
    }
}
