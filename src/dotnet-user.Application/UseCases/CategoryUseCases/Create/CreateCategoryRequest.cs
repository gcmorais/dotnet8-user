using dotnet_user_api.Application.UseCases.CategoryUseCases.Common;
using MediatR;

namespace dotnet_user_api.Application.UseCases.CategoryUseCases.Create
{
    public sealed record CreateCategoryRequest : IRequest<CategoryResponse>
    {
        public string Name { get; set; }
    }
}
