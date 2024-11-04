using dotnet8_user.Application.Interfaces;
using dotnet8_user.Domain.Interfaces;
using MediatR;

namespace dotnet8_user.Application.UseCases.UserUseCases.Login
{
    public class UserLoginHandler : IRequestHandler<UserLoginRequest, UserLoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ICreateVerifyHash _serviceHash;

        public UserLoginHandler(IUserRepository userRepository, ITokenService tokenService, ICreateVerifyHash serviceHash)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _serviceHash = serviceHash;
        }

        public async Task<UserLoginResponse> Handle(UserLoginRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUserName(request.UserName, cancellationToken);

            if (user == null || !_serviceHash.PasswordVerify(request.Password, user.HashPassword, user.SaltPassword))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            var token = _tokenService.GenerateToken(user, user.Roles);
            return new UserLoginResponse { Token = token };
        }
    }
}
