using AutoMapper;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Domain.Interfaces;
using MediatR;

namespace dotnet8_user.Application.UseCases.AdminUseCases.Assign
{
    public class AssignAdminRoleHandler : IRequestHandler<AssignAdminRoleRequest, UserResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ICreateVerifyHash _createVerifyHash;
        public AssignAdminRoleHandler(
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
        public async Task<UserResponse> Handle(AssignAdminRoleRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Get(request.id, cancellationToken);

            if (user == null) throw new InvalidOperationException("User not found.");

            _userRepository.AssignRole(user.Id, "Admin", cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            return _mapper.Map<UserResponse>(user);
        }
    }
}
