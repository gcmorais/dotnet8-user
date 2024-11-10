using AutoFixture;
using AutoMapper;
using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using dotnet8_user.Application.UseCases.CategoryUseCases.Create;
using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Application.UseCases.UserUseCases.Create;
using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using Moq;
using Shouldly;

namespace Application.UnitTests.UseCases.CategoryUseCases
{
    public class CategoryCreateUnitTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICreateVerifyHash> _createVerifyHashMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

        public CategoryCreateUnitTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _createVerifyHashMock = new Mock<ICreateVerifyHash>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
        }
        [Fact]
        public async Task ValidCategory_CreateIsCalled_ReturnValidResponseCategory()
        {
            // Arrange
            var userCreateRequest = new Fixture().Create<UserCreateRequest>();

            byte[] hashPassword = new byte[32];
            byte[] saltPassword = new byte[16];

            var userId = Guid.NewGuid();

            var userWithId = new User(
                email: userCreateRequest.Email,
                fullname: userCreateRequest.FullName,
                username: userCreateRequest.UserName,
                hashPassword: hashPassword,
                saltPassword: saltPassword
            );
            typeof(User).GetProperty("Id")?.SetValue(userWithId, userId);

            var categoryCreateRequest = new Fixture().Create<CategoryCreateRequest>();

            var category = new Category(
                name: categoryCreateRequest.Name,
                description: categoryCreateRequest.Description,
                user: userWithId
            );

            var categoryResponse = new CategoryResponse
            {
                Id = category.Id,
                Name = categoryCreateRequest.Name,
                Description = categoryCreateRequest.Description,
                User = new UserShortResponse
                {
                    Id = userWithId.Id,
                    Email = userWithId.Email,
                    UserName = userWithId.UserName,
                },
                Products = new List<ProductShortResponse>()
            };

            var cancellationToken = new CancellationToken();

            _userRepositoryMock
                .Setup(repo => repo.Get(categoryCreateRequest.UserId, cancellationToken))
                .ReturnsAsync(userWithId);

            _mapperMock
                .Setup(m => m.Map<Category>(categoryCreateRequest))
                .Returns(category);

            _mapperMock
                .Setup(m => m.Map<CategoryResponse>(It.IsAny<Category>()))
                .Returns(categoryResponse);

            _categoryRepositoryMock
                .Setup(repo => repo.Create(It.IsAny<Category>()))
                .Callback<Category>(c =>
                {
                    c.Name.ShouldBe(category.Name);
                    c.Description.ShouldBe(category.Description);
                    c.UserId.ShouldBe(category.UserId);
                    c.User.ShouldBe(userWithId);
                });

            var categoryCreateHandler = new CategoryCreateHandler(
                _unitOfWorkMock.Object,
                _categoryRepositoryMock.Object,
                _mapperMock.Object,
                _userRepositoryMock.Object
            );

            // Act & Assert
            var response = await categoryCreateHandler.Handle(categoryCreateRequest, cancellationToken);

            response.ShouldNotBeNull();
            response.Id.ShouldBe(category.Id);
            response.Name.ShouldBe(category.Name);
            response.Description.ShouldBe(category.Description);

            _userRepositoryMock.Verify(repo => repo.Get(categoryCreateRequest.UserId, cancellationToken), Times.Once);
            _categoryRepositoryMock.Verify(repo => repo.Create(It.IsAny<Category>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task InvalidUserId_ThrowsInvalidOperationException()
        {
            // Arrange
            var categoryCreateRequest = new Fixture().Create<CategoryCreateRequest>();
            var cancellationToken = new CancellationToken();

            _userRepositoryMock
                .Setup(repo => repo.Get(categoryCreateRequest.UserId, cancellationToken))
                .ReturnsAsync((User)null);

            var categoryCreateHandler = new CategoryCreateHandler(
                _unitOfWorkMock.Object,
                _categoryRepositoryMock.Object,
                _mapperMock.Object,
                _userRepositoryMock.Object
            );

            // Act & Assert
            await Should.ThrowAsync<InvalidOperationException>(async () =>
                await categoryCreateHandler.Handle(categoryCreateRequest, cancellationToken));

            _userRepositoryMock.Verify(repo => repo.Get(categoryCreateRequest.UserId, cancellationToken), Times.Once);

            _categoryRepositoryMock.Verify(repo => repo.Create(It.IsAny<Category>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Never);
        }

    }
}
