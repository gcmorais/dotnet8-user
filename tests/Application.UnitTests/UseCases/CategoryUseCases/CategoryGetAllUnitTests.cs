using AutoMapper;
using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using dotnet8_user.Application.UseCases.CategoryUseCases.GetAll;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using Moq;
using Shouldly;

namespace Application.UnitTests.UseCases.CategoryUseCases
{
    public class CategoryGetAllUnitTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public CategoryGetAllUnitTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetAllCategories_Repository_ReturnsCategoryResponse()
        {
            // Arrange
            byte[] hashPassword = new byte[32];
            byte[] saltPassword = new byte[16];

            var user = new User("Fullname", "Username", "user@example.com", hashPassword, saltPassword);

            var userShortResponse = new UserShortResponse
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName
            };

            var categories = new List<Category>
            {
                new Category(name: "Category1", description: "Test category1", user: user),
                new Category(name: "Category2", description: "Test category2", user: user)
            };

            var categoryResponse = new List<CategoryResponse>
            {
                new CategoryResponse { Name = "Category1", Description = "Test category1", User = userShortResponse },
                new CategoryResponse { Name = "Category2", Description = "Test category2", User = userShortResponse }
            };

            var cancellationToken = new CancellationToken();

            _categoryRepositoryMock
                .Setup(repo => repo.GetAll(cancellationToken))
                .ReturnsAsync(categories);

            _mapperMock
                .Setup(m => m.Map<List<CategoryResponse>>(categories))
                .Returns(categoryResponse);

            var categoryGetAllHandler = new CategoryGetAllHandler(_categoryRepositoryMock.Object, _mapperMock.Object);

            // Act
            var response = await categoryGetAllHandler.Handle(new CategoryGetAllRequest(), cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Count.ShouldBe(categoryResponse.Count);

            for (int i = 0; i < response.Count; i++)
            {
                response[i].Name.ShouldBe(categoryResponse[i].Name);
                response[i].Description.ShouldBe(categoryResponse[i].Description);
                response[i].User.ShouldBe(userShortResponse);
            }

            _categoryRepositoryMock.Verify(repo => repo.GetAll(cancellationToken), Times.Once);
            _mapperMock.Verify(m => m.Map<List<CategoryResponse>>(categories), Times.Once);
        }

        [Fact]
        public async Task GetAllCategories_Repository_ReturnsEmptyList()
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            _categoryRepositoryMock
                .Setup(repo => repo.GetAll(cancellationToken))
                .ReturnsAsync(new List<Category>());

            var categoryGetAllHandler = new CategoryGetAllHandler(_categoryRepositoryMock.Object, _mapperMock.Object);

            // Act & Assert
            var response = await categoryGetAllHandler.Handle(new CategoryGetAllRequest(), cancellationToken);

            response.ShouldBeEmpty();
            _categoryRepositoryMock.Verify(repo => repo.GetAll(cancellationToken), Times.Once);
        }
    }
}
