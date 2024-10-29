using AutoMapper;
using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using dotnet8_user.Domain.Interfaces;
using MediatR;

namespace dotnet8_user.Application.UseCases.ProductUseCases.Update
{
    public class ProductUpdateHandler : IRequestHandler<ProductUpdateRequest, ProductResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductsRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductUpdateHandler(IUnitOfWork unitOfWork, IProductsRepository productsRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productsRepository;
            _mapper = mapper;
        }
        public async Task<ProductResponse> Handle(ProductUpdateRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.Get(request.Id, cancellationToken);

            if (product == null) throw new InvalidOperationException($"Product with ID {request.Id} not found.");

            product.UpdateName(request.Name);
            product.UpdateDescription(request.Description);
            product.UpdatePrice(request.Price);
            product.UpdateStock(request.StockQuantity);
            product.UpdateBrand(request.Brand);
            product.UpdateImage(request.ImageUrl);

            _productRepository.Update(product);

            await _unitOfWork.Commit(cancellationToken);
            return _mapper.Map<ProductResponse>(product);
        }
    }
}
