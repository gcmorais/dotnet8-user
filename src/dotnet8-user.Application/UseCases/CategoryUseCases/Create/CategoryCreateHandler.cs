using AutoMapper;
using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using MediatR;

namespace dotnet8_user.Application.UseCases.CategoryUseCases.Create
{
    public class CategoryCreateHandler : IRequestHandler<CategoryCreateRequest, CategoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public CategoryCreateHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IMapper mapper, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<CategoryResponse> Handle(CategoryCreateRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Get(request.UserId, cancellationToken);

            if (user == null) throw new InvalidOperationException("User not found.");

            var category = _mapper.Map<Category>(request);

            category.AssignUser(user);

            _categoryRepository.Create(category);

            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<CategoryResponse>(category);
        }
    }
}
