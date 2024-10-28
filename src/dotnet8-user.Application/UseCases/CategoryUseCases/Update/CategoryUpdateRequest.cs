using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using MediatR;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.Update
{
    public sealed record CategoryUpdateRequest : IRequest<CategoryResponse>, ICategoryRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
    }
}
