using AutoMapper;
using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using dotnet8_user.Domain.Interfaces;
using MediatR;

namespace dotnet8_user.Application.UseCases.ProductUseCases.Deactivate
{
    public class ProductDeactivateHandler : IRequestHandler<ProductDeactivateRequest, ProductResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductsRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductDeactivateHandler(IUnitOfWork unitOfWork, IProductsRepository productsRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productsRepository;
            _mapper = mapper;
        }
        public async Task<ProductResponse> Handle(ProductDeactivateRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.Get(request.Id, cancellationToken);

            if (product == null) throw new InvalidOperationException($"Product with ID {request.Id} not found.");

            product.Deactivate();

            await _unitOfWork.Commit(cancellationToken);
            return _mapper.Map<ProductResponse>(product);
        }
    }
}
