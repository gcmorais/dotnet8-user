using dotnet8_user.Application.UseCases.UserUseCases.Common;
using MediatR;

namespace dotnet8_user.Application.UseCases.AdminUseCases.Assign
{
    public sealed record AssignAdminRoleRequest(Guid id) : IRequest<UserResponse>;
}
