using AutoMapper;
using dotnet8_user.Application.UseCases.AdminUseCases.Common;
using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using MediatR;

namespace dotnet8_user.Application.UseCases.AdminUseCases.Create
{
    public class AdminCreateHandler : IRequestHandler<AdminCreateRequest, AdminResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ICreateVerifyHash _createVerifyHash;
        public AdminCreateHandler(
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

        public async Task<AdminResponse> Handle(AdminCreateRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmail(request.Email, cancellationToken);

            if (existingUser != null) throw new InvalidOperationException("User with the same email already exists.");

            _createVerifyHash.CreateHashPassword(request.Password, out byte[] hashPassword, out byte[] saltPassword);

            var user = new User(
               request.FullName,
               request.UserName,
               request.Email,
               hashPassword,
               saltPassword
            );

            await _userRepository.CreateAdmin(user, new List<string> { "Admin" }, cancellationToken);

            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<AdminResponse>(user);
        }
    }
}
