using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using MediatR;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.GetAll
{
    public sealed record CategoryGetAllRequest : IRequest<List<CategoryResponse>>;
}
