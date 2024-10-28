using AutoMapper;
using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using dotnet8_user.Domain.Interfaces;
using MediatR;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.Update
{
    public class CategoryUpdateHandler : IRequestHandler<CategoryUpdateRequest, CategoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryUpdateHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<CategoryResponse> Handle(CategoryUpdateRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.Get(request.Id, cancellationToken);

            if (category == null) throw new InvalidOperationException($"Category with ID {request.Id} not found.");

            category.UpdateName(request.Name);
            category.UpdateDescription(request.Description);
            category.UpdateDate();

            _categoryRepository.Update(category);

            await _unitOfWork.Commit(cancellationToken);
            return _mapper.Map<CategoryResponse>(category);
        }
    }
}
