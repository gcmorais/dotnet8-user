using AutoMapper;
using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using dotnet8_user.Domain.Interfaces;
using MediatR;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.GetAll
{
    public class CategoryGetAllHandler : IRequestHandler<CategoryGetAllRequest, List<CategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryGetAllHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<List<CategoryResponse>> Handle(CategoryGetAllRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetAll(cancellationToken);
            return _mapper.Map<List<CategoryResponse>>(category);
        }
    }
}
