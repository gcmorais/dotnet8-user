using AutoMapper;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Domain.Interfaces;
using MediatR;

namespace dotnet8_user.Application.UseCases.UserUseCases.GetAll
{
    public class GetAllUserHandler : IRequestHandler<GetAllUserRequest, List<UserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserResponse>> Handle(GetAllUserRequest request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAll(cancellationToken);

            if (users == null)
                return new List<UserResponse>();

            return _mapper.Map<List<UserResponse>>(users);
        }
    }
}
