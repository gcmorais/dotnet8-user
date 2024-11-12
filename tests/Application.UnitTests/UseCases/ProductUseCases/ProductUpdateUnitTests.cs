using AutoMapper;
using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using dotnet8_user.Application.UseCases.ProductUseCases.Update;
using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using Moq;
using Shouldly;

namespace Application.UnitTests.UseCases.ProductUseCases
{
    public class ProductUpdateUnitTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProductsRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public ProductUpdateUnitTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productRepositoryMock = new Mock<IProductsRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task ProductUpdate_ValidRequest_UpdatesProductAndReturnsResponse()
        {
            // Arrange
            var user = new User("User test", "usertest", "user@example.com", new byte[32], new byte[16]);
            var existingCategory = new Category("Category 1", "Category Description", user);
            var existingProduct = new Products("Product 1", "Description 1", 100, 10, "Brand 1", "http://imageurl.com/1", existingCategory)
            {
                Id = Guid.NewGuid()
            };

            var productUpdateRequest = new ProductUpdateRequest
            {
                Id = existingProduct.Id,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 150,
                StockQuantity = 20,
                Brand = "Updated Brand",
                ImageUrl = "http://newimageurl.com/1",
                CategoryId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            var updatedProduct = new Products("Updated Product", "Updated Description", 150, 20, "Updated Brand", "http://newimageurl.com/1", existingCategory)
            {
                Id = existingProduct.Id
            };

            var productResponse = new ProductResponse
            {
                Id = existingProduct.Id,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 150,
                StockQuantity = 20,
                Brand = "Updated Brand",
                ImageUrl = "http://newimageurl.com/1"
            };

            _productRepositoryMock.Setup(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            _productRepositoryMock.Setup(repo => repo.Update(It.IsAny<Products>()));

            _mapperMock.Setup(mapper => mapper.Map<ProductResponse>(It.IsAny<Products>()))
                .Returns(productResponse);

            var handler = new ProductUpdateHandler(_unitOfWorkMock.Object, _productRepositoryMock.Object, _mapperMock.Object);

            // Act & Assert
            var response = await handler.Handle(productUpdateRequest, CancellationToken.None);

            response.ShouldBe(productResponse);

            _productRepositoryMock.Verify(repo => repo.Get(existingProduct.Id, It.IsAny<CancellationToken>()), Times.Once);
            _productRepositoryMock.Verify(repo => repo.Update(It.IsAny<Products>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(m => m.Map<ProductResponse>(It.IsAny<Products>()), Times.Once);
        }

        [Fact]
        public async Task ProductUpdate_ProductNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var productUpdateRequest = new ProductUpdateRequest
            {
                Id = Guid.NewGuid(),
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 150,
                StockQuantity = 20,
                Brand = "Updated Brand",
                ImageUrl = "http://newimageurl.com/1",
                CategoryId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            _productRepositoryMock.Setup(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Products)null);

            var handler = new ProductUpdateHandler(_unitOfWorkMock.Object, _productRepositoryMock.Object, _mapperMock.Object);

            // Act & Assert
            var exception = await Should.ThrowAsync<InvalidOperationException>(
                async () => await handler.Handle(productUpdateRequest, CancellationToken.None)
            );

            exception.Message.ShouldBe($"Product with ID {productUpdateRequest.Id} not found.");

            _productRepositoryMock.Verify(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _productRepositoryMock.Verify(repo => repo.Update(It.IsAny<Products>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(m => m.Map<ProductResponse>(It.IsAny<Products>()), Times.Never);
        }
    }
}
