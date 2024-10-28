using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using MediatR;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.Update
{
    public sealed record CategoryUpdateRequest(Guid Id, string Name, string Description) : IRequest<CategoryResponse>;
}
