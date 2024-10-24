using AutoMapper;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Domain.Interfaces;
using MediatR;

namespace dotnet8_user.Application.UseCases.UserUseCases.Delete
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, UserResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public DeleteUserHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserResponse> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Get(request.id, cancellationToken);

            if (user == null) throw new InvalidOperationException("User not found.");

            _userRepository.Delete(user);
            await _unitOfWork.Commit(cancellationToken);
            return _mapper.Map<UserResponse>(user);

        }
    }
}
