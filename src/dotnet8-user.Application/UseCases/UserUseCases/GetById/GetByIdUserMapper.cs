using AutoMapper;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.UseCases.UserUseCases.GetById
{
    public sealed class GetByIdUserMapper : Profile
    {
        public GetByIdUserMapper()
        {
            CreateMap<GetByIdUserRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }
}
