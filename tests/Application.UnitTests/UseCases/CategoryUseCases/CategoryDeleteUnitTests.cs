using AutoFixture;
using AutoMapper;
using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using dotnet8_user.Application.UseCases.CategoryUseCases.Delete;
using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using Moq;
using Shouldly;

namespace Application.UnitTests.UseCases.CategoryUseCases
{
    public class CategoryDeleteUnitTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

        public CategoryDeleteUnitTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
        }

        [Fact]
        public async Task CategoryExists_DeleteIsCalled_ReturnsValidCategoryResponse()
        {
            // Arrange
            var categoryDeleteRequest = new Fixture().Create<CategoryDeleteRequest>();
            var cancellationToken = new CancellationToken();

            byte[] hashPassword = new byte[32];
            byte[] saltPassword = new byte[16];

            var category = new Category(
                name: "Category",
                description: "Test category",
                user: new User("Fullname", "Username", "user@example.com", hashPassword, saltPassword)
            );

            _categoryRepositoryMock
                .Setup(repo => repo.Get(categoryDeleteRequest.Id, cancellationToken))
                .ReturnsAsync(category);

            _categoryRepositoryMock
                .Setup(repo => repo.Delete(category))
                .Verifiable();

            _mapperMock
                .Setup(m => m.Map<CategoryResponse>(category))
                .Returns(new CategoryResponse
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                });

            var categoryDeleteHandler = new CategoryDeleteHandler(
                _unitOfWorkMock.Object,
                _categoryRepositoryMock.Object,
                _mapperMock.Object
            );

            // Act
            var response = await categoryDeleteHandler.Handle(categoryDeleteRequest, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Id.ShouldBe(category.Id);
            response.Name.ShouldBe(category.Name);
            response.Description.ShouldBe(category.Description);

            _categoryRepositoryMock.Verify(repo => repo.Delete(category), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task CategoryDoesNotExist_DeleteIsCalled_ThrowsInvalidOperationException()
        {
            // Arrange
            var categoryDeleteRequest = new Fixture().Create<CategoryDeleteRequest>();
            var cancellationToken = new CancellationToken();

            _categoryRepositoryMock
                .Setup(repo => repo.Get(categoryDeleteRequest.Id, cancellationToken))
                .ReturnsAsync((Category)null);

            var categoryDeleteHandler = new CategoryDeleteHandler(
                _unitOfWorkMock.Object,
                _categoryRepositoryMock.Object,
                _mapperMock.Object
            );

            // Act & Assert
            await Should.ThrowAsync<InvalidOperationException>(async () =>
            {
                await categoryDeleteHandler.Handle(categoryDeleteRequest, cancellationToken);
            });

            _categoryRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Category>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Never);
        }
    }
}
