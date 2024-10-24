using AutoMapper;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Domain.Entities;

namespace dotnet8_user.Application.UseCases.UserUseCases.Delete
{
    public sealed class DeleteUserMapper : Profile
    {
        public DeleteUserMapper()
        {
            CreateMap<DeleteUserRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }
}
