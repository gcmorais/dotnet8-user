using AutoMapper;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Domain.Interfaces;
using MediatR;

namespace dotnet8_user.Application.UseCases.UserUseCases.GetById
{
    public class GetByIdUserHandler : IRequestHandler<GetByIdUserRequest, UserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetByIdUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserResponse> Handle(GetByIdUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.id, cancellationToken);

            if (user == null)
                throw new KeyNotFoundException($"User with ID {request.id} not found.");

            return _mapper.Map<UserResponse>(user);
        }
    }
}
