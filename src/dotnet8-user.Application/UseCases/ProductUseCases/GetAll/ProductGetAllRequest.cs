using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using MediatR;

namespace dotnet8_user.Application.UseCases.ProductUseCases.GetAll
{
    public sealed record ProductGetAllRequest : IRequest<List<ProductResponse>>;
}
