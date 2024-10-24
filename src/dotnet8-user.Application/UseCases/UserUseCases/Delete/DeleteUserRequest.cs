using dotnet8_user.Application.UseCases.UserUseCases.Common;
using MediatR;

namespace dotnet8_user.Application.UseCases.UserUseCases.Delete
{
    public sealed record DeleteUserRequest(Guid id) : IRequest<UserResponse>;
}
