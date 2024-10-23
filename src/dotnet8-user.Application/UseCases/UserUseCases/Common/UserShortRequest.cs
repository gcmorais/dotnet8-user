using MediatR;

namespace dotnet8_user.Application.UseCases.UserUseCases.Common
{
    public sealed record UserShortRequest(Guid Id, string Email, string FullName) : IRequest<UserShortResponse>;
}
