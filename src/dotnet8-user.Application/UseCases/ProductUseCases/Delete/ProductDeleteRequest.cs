using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using MediatR;

namespace dotnet8_user.Application.UseCases.ProductUseCases.Delete
{
    public sealed record ProductDeleteRequest(Guid Id) : IRequest<ProductResponse>;
}
