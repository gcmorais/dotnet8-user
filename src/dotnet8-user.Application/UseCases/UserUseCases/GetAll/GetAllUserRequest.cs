using dotnet8_user.Application.UseCases.UserUseCases.Common;
using MediatR;

namespace dotnet8_user.Application.UseCases.UserUseCases.GetAll
{
    public sealed record GetAllUserRequest : IRequest<List<UserResponse>>;
}
