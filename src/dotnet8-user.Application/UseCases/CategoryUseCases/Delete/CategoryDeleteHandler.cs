using AutoMapper;
using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using dotnet8_user.Domain.Interfaces;
using MediatR;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.Delete
{
    public class CategoryDeleteHandler : IRequestHandler<CategoryDeleteRequest, CategoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryDeleteHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<CategoryResponse> Handle(CategoryDeleteRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.Get(request.Id, cancellationToken);

            if (category == null) throw new InvalidOperationException($"Category with ID {request.Id} not found.");

            _categoryRepository.Delete(category);
            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<CategoryResponse>(category);
        }
    }
}
