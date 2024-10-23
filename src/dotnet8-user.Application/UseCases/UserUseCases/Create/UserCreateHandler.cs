using AutoMapper;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using MediatR;

namespace dotnet8_user.Application.UseCases.UserUseCases.Create
{
    public class UserCreateHandler : IRequestHandler<UserCreateRequest, UserResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ICreateVerifyHash _createVerifyHash;
        public UserCreateHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICreateVerifyHash createVerifyHash)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mapper = mapper;
            _createVerifyHash = createVerifyHash;
        }
        public async Task<UserResponse> Handle(UserCreateRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmail(request.Email, cancellationToken);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with the same email already exists.");
            }

            _createVerifyHash.CreateHashPassword(request.Password, out byte[] hashPassword, out byte[] saltPassword);

            var user = new User(
                request.FullName,
                request.UserName,
                request.Email,
                hashPassword,
                saltPassword
            );

            _userRepository.Create(user);

            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<UserResponse>(user);
        }
    }
}
