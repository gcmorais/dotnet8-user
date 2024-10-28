using System.ComponentModel.DataAnnotations;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using MediatR;

namespace dotnet8_user.Application.UseCases.UserUseCases.Create
{
    public sealed record UserCreateRequest : IRequest<UserResponse>, IUserRequest
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords are not the same")]
        public string ConfirmPassword { get; set; }
    };
}
