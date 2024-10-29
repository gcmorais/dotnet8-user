using AutoMapper;
using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using dotnet8_user.Domain.Interfaces;
using MediatR;

namespace dotnet8_user.Application.UseCases.ProductUseCases.Delete
{
    public class ProductDeleteHandler : IRequestHandler<ProductDeleteRequest, ProductResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductsRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductDeleteHandler(IUnitOfWork unitOfWork, IProductsRepository productsRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productsRepository;
            _mapper = mapper;
        }
        public async Task<ProductResponse> Handle(ProductDeleteRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.Get(request.Id, cancellationToken);

            if (product == null) throw new InvalidOperationException($"Product with ID {request.Id} not found.");

            _productRepository.Delete(product);
            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<ProductResponse>(product);
        }
    }
}
