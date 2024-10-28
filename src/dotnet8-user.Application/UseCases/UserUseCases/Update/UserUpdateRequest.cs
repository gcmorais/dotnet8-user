using System.ComponentModel.DataAnnotations;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using MediatR;

namespace dotnet8_user.Application.UseCases.UserUseCases.Update
{
    public sealed record UserUpdateRequest() : IRequest<UserResponse>, IUserRequest
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
    }
}
