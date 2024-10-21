using AutoMapper;
using dotnet_user_api.Application.UseCases.CategoryUseCases.Common;
using dotnet_user_api.Domain.Entities;
using dotnet_user_api.Domain.Interfaces;
using MediatR;

namespace dotnet_user_api.Application.UseCases.CategoryUseCases.Create
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryRequest, CategoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CreateCategoryHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<CategoryResponse> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request);

            _categoryRepository.Create(category);

            await _unitOfWork.Commit(cancellationToken);
            return _mapper.Map<CategoryResponse>(category);
        }
    }
}
