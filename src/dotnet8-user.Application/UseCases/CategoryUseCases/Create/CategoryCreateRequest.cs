using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using MediatR;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.Create
{
    public sealed record CategoryCreateRequest : IRequest<CategoryResponse>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
    }
}
