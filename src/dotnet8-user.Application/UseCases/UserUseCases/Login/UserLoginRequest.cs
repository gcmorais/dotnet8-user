using MediatR;

namespace dotnet8_user.Application.UseCases.UserUseCases.Login
{
    public class UserLoginRequest : IRequest<UserLoginResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
