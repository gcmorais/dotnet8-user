using AutoMapper;
using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using MediatR;

namespace dotnet8_user.Application.UseCases.ProductUseCases.Create
{
    public class ProductCreateHandler : IRequestHandler<ProductCreateRequest, ProductResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductsRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public ProductCreateHandler(IUnitOfWork unitOfWork, IProductsRepository productsRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productsRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<ProductResponse> Handle(ProductCreateRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.Get(request.CategoryId, cancellationToken);

            if (category == null) throw new InvalidOperationException("Category not found.");

            var product = _mapper.Map<Products>(request);

            product.AssignCategory(category);

            _productRepository.Create(product);

            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<ProductResponse>(product);
        }
    }
}
