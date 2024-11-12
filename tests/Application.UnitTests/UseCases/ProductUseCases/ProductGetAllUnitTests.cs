using AutoMapper;
using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using dotnet8_user.Application.UseCases.ProductUseCases.GetAll;
using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using Moq;
using Shouldly;

namespace Application.UnitTests.UseCases.ProductUseCases
{
    public class ProductGetAllUnitTests
    {
        private readonly Mock<IProductsRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public ProductGetAllUnitTests()
        {
            _productRepositoryMock = new Mock<IProductsRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetAllProducts_Repository_ReturnsProductsResponse()
        {
            // Arrange
            byte[] hashPassword = new byte[32];
            byte[] saltPassword = new byte[16];

            var user = new User("User test", "usertest", "user@example.com", hashPassword, saltPassword);
            user.AddRole("User");

            var category = new Category("Category 1", "Category Description", user);

            var products = new List<Products>
            {
                new Products("Product 1", "Description 1", 100, 10, "Brand 1", "http://imageurl.com/1", category),
                new Products("Product 2", "Description 2", 200, 20, "Brand 2", "http://imageurl.com/2", category)
            };

            var productResponses = new List<ProductGetAllResponse>
            {
                new ProductGetAllResponse { Id = products[0].Id, Name = "Product 1", Price = 100, StockQuantity = 10, Brand = "Brand 1" },
                new ProductGetAllResponse { Id = products[1].Id, Name = "Product 2", Price = 200, StockQuantity = 20, Brand = "Brand 2" }
            };

            _productRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            _mapperMock.Setup(mapper => mapper.Map<List<ProductGetAllResponse>>(products))
                .Returns(productResponses);

            var handler = new ProductGetAllHandler(_productRepositoryMock.Object, _mapperMock.Object);
            var cancellationToken = new CancellationToken();

            // Act & Assert
            var response = await handler.Handle(new ProductGetAllRequest(), CancellationToken.None);

            response.ShouldNotBeNull();
            response.ShouldBeOfType<List<ProductGetAllResponse>>();
            response.Count.ShouldBe(productResponses.Count);

            for (int i = 0; i < response.Count; i++)
            {
                response[i].Id.ShouldBe(productResponses[i].Id);
                response[i].Name.ShouldBe(productResponses[i].Name);
                response[i].Description.ShouldBe(productResponses[i].Description);
                response[i].Price.ShouldBe(productResponses[i].Price);
                response[i].Brand.ShouldBe(productResponses[i].Brand);
            }

            _productRepositoryMock.Verify(repo => repo.GetAll(cancellationToken), Times.Once);
            _mapperMock.Verify(m => m.Map<List<ProductGetAllResponse>>(products), Times.Once);
        }

        [Fact]
        public async Task GetAllProducts_Repository_ReturnsEmptyList()
        {
            // Arrange
            byte[] hashPassword = new byte[32];
            byte[] saltPassword = new byte[16];

            var user = new User("User test", "usertest", "user@example.com", hashPassword, saltPassword);
            user.AddRole("User");

            var category = new Category("Category 1", "Category Description", user);

            var products = new List<Products>();

            var productResponses = new List<ProductGetAllResponse>();

            _productRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            _mapperMock.Setup(mapper => mapper.Map<List<ProductGetAllResponse>>(products))
                .Returns(productResponses);

            var handler = new ProductGetAllHandler(_productRepositoryMock.Object, _mapperMock.Object);
            var cancellationToken = new CancellationToken();

            // Act & Assert
            var result = await handler.Handle(new ProductGetAllRequest(), CancellationToken.None);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<List<ProductGetAllResponse>>();
            result.Count.ShouldBe(0);

            _productRepositoryMock.Verify(repo => repo.GetAll(cancellationToken), Times.Once);
            _mapperMock.Verify(m => m.Map<List<ProductGetAllResponse>>(products), Times.Once);
        }

    }
}
