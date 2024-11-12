using AutoFixture;
using AutoMapper;
using dotnet8_user.Application.UseCases.CategoryUseCases.Create;
using dotnet8_user.Application.UseCases.ProductUseCases.Create;
using dotnet8_user.Application.UseCases.ProductUseCases.Delete;
using dotnet8_user.Application.UseCases.UserUseCases.Create;
using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using Moq;

namespace Application.UnitTests.UseCases.ProductUseCases
{
    public class ProductDeleteUnitTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IProductsRepository> _productRepositoryMock;

        public ProductDeleteUnitTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _productRepositoryMock = new Mock<IProductsRepository>();
        }

        [Fact]
        public async Task ProductExists_DeleteIsCalled_DeletesProductSuccessfully()
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

            var productCreateRequest = new Fixture().Create<ProductCreateRequest>();

            var product = new Products(
                name: productCreateRequest.Name,
                description: productCreateRequest.Description,
                price: productCreateRequest.Price,
                stockquantity: productCreateRequest.StockQuantity,
                brand: productCreateRequest.Brand,
                imageurl: productCreateRequest.ImageUrl,
                category: category
            );

            _productRepositoryMock.Setup(repo => repo.Get(product.Id, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(product);

            _productRepositoryMock.Setup(repo => repo.Delete(It.IsAny<Products>())).Verifiable();

            var handler = new ProductDeleteHandler(
                _unitOfWorkMock.Object,
                _productRepositoryMock.Object,
                _mapperMock.Object
            );

            // Act & Assert
            await handler.Handle(new ProductDeleteRequest(product.Id), CancellationToken.None);

            _productRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Products>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ProductDoesNotExist_DeleteIsNotCalled_ThrowsException()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _productRepositoryMock.Setup(repo => repo.Get(productId, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync((Products)null);

            var handler = new ProductDeleteHandler(
                _unitOfWorkMock.Object,
                _productRepositoryMock.Object,
                _mapperMock.Object
            );

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await handler.Handle(new ProductDeleteRequest(productId), CancellationToken.None);
            });
        }
    }
}
