using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using MediatR;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.Delete
{
    public sealed record CategoryDeleteRequest(Guid Id) : IRequest<CategoryResponse>;
}
