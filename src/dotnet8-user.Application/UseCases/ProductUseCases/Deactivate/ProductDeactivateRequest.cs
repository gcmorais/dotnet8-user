using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using MediatR;

namespace dotnet8_user.Application.UseCases.ProductUseCases.Deactivate
{
    public sealed record ProductDeactivateRequest(Guid Id) : IRequest<ProductResponse>;
}
