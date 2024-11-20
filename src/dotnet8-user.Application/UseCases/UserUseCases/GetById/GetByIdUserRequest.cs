using dotnet8_user.Application.UseCases.UserUseCases.Common;
using MediatR;

namespace dotnet8_user.Application.UseCases.UserUseCases.GetById
{
    public sealed record GetByIdUserRequest(Guid id) : IRequest<UserResponse>;
}
