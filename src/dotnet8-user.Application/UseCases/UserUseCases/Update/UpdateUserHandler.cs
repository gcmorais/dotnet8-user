using AutoMapper;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Domain.Interfaces;
using MediatR;

namespace dotnet8_user.Application.UseCases.UserUseCases.Update
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, UserResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUserHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Get(request.Id, cancellationToken);

            if (user == null) throw new InvalidOperationException("User not found.");

            if (!string.IsNullOrWhiteSpace(request.FullName))
            {
                user.UpdateName(request.FullName);
            }

            if (!string.IsNullOrWhiteSpace(request.UserName))
            {
                user.UpdateUsername(request.UserName);
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                user.UpdateEmail(request.Email);
            }

            _userRepository.Update(user);

            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<UserResponse>(user);
        }
    }
}
