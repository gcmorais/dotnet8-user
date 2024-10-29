using AutoMapper;
using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using dotnet8_user.Domain.Interfaces;
using MediatR;

namespace dotnet8_user.Application.UseCases.ProductUseCases.GetAll
{
    public class ProductGetAllHandler : IRequestHandler<ProductGetAllRequest, List<ProductGetAllResponse>>
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IMapper _mapper;
        public ProductGetAllHandler(IProductsRepository productsRepository, IMapper mapper)
        {
            _productsRepository = productsRepository;
            _mapper = mapper;
        }
        public async Task<List<ProductGetAllResponse>> Handle(ProductGetAllRequest request, CancellationToken cancellationToken)
        {
            var products = await _productsRepository.GetAll(cancellationToken);
            return _mapper.Map<List<ProductGetAllResponse>>(products);
        }
    }
}
