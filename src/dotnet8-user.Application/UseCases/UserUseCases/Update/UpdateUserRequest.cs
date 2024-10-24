using dotnet8_user.Application.UseCases.UserUseCases.Common;
using MediatR;

namespace dotnet8_user.Application.UseCases.UserUseCases.Update
{
    public sealed record UpdateUserRequest(Guid Id, string Email, string FullName, string UserName) : IRequest<UserResponse>;
}
