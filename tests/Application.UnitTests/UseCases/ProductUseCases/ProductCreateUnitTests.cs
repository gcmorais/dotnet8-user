using AutoFixture;
using AutoMapper;
using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using dotnet8_user.Application.UseCases.CategoryUseCases.Create;
using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using dotnet8_user.Application.UseCases.ProductUseCases.Create;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Application.UseCases.UserUseCases.Create;
using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using Moq;
using Shouldly;

namespace Application.UnitTests.UseCases.ProductUseCases
{
    public class ProductCreateUnitTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICreateVerifyHash> _createVerifyHashMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IProductsRepository> _productRepositoryMock;

        public ProductCreateUnitTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _createVerifyHashMock = new Mock<ICreateVerifyHash>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _productRepositoryMock = new Mock<IProductsRepository>();
        }

        [Fact]
        public async Task ValidProduct_CreateIsCalled_ReturnValidResponseProduct()
        {
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

            var productResponse = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Brand = product.Brand,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive,
                Category = new CategoryResponse
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    User = new UserShortResponse
                    {
                        Id = userWithId.Id,
                        Email = userWithId.Email,
                        UserName = userWithId.UserName,
                    }
                }
            };

            var cancellationToken = new CancellationToken();

            _categoryRepositoryMock
                .Setup(repo => repo.Get(productCreateRequest.CategoryId, cancellationToken))
                .ReturnsAsync(category);

            _mapperMock
               .Setup(m => m.Map<Category>(categoryCreateRequest))
               .Returns(category);

            _mapperMock
               .Setup(m => m.Map<Products>(productCreateRequest))
               .Returns(product);

            _mapperMock
                .Setup(m => m.Map<ProductResponse>(It.IsAny<Products>()))
                .Returns(productResponse);

            _productRepositoryMock
                .Setup(repo => repo.Create(It.IsAny<Products>()))
                .Callback<Products>(c =>
                {
                    c.Name.ShouldBe(product.Name);
                    c.Description.ShouldBe(product.Description);
                    c.Price.ShouldBe(product.Price);
                    c.StockQuantity.ShouldBe(product.StockQuantity);
                    c.Brand.ShouldBe(product.Brand);
                    c.ImageUrl.ShouldBe(product.ImageUrl);
                });

            var productCreateHandler = new ProductCreateHandler(
               _unitOfWorkMock.Object,
               _productRepositoryMock.Object,
               _categoryRepositoryMock.Object,
               _mapperMock.Object
            );

            // Act & Assert
            var response = await productCreateHandler.Handle(productCreateRequest, cancellationToken);

            response.ShouldNotBeNull();
            response.Id.ShouldBe(product.Id);
            response.Name.ShouldBe(product.Name);
            response.Description.ShouldBe(product.Description);
            response.Price.ShouldBe(product.Price);
            response.StockQuantity.ShouldBe(product.StockQuantity);
            response.Brand.ShouldBe(product.Brand);
            response.ImageUrl.ShouldBe(product.ImageUrl);

            _categoryRepositoryMock.Verify(repo => repo.Get(productCreateRequest.CategoryId, cancellationToken), Times.Once);
            _productRepositoryMock.Verify(repo => repo.Create(It.IsAny<Products>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task InvalidCategoryId_ThrowsInvalidOperationException()
        {
            // Arrange
            var productCreateRequest = new Fixture().Create<ProductCreateRequest>();
            var cancellationToken = new CancellationToken();

            _categoryRepositoryMock
                .Setup(repo => repo.Get(productCreateRequest.CategoryId, cancellationToken))
                .ReturnsAsync((Category)null);

            var productCreateHandler = new ProductCreateHandler(
               _unitOfWorkMock.Object,
               _productRepositoryMock.Object,
               _categoryRepositoryMock.Object,
               _mapperMock.Object
            );

            // Act & Assert
            await Should.ThrowAsync<InvalidOperationException>(async () =>
                await productCreateHandler.Handle(productCreateRequest, cancellationToken));

            _categoryRepositoryMock.Verify(repo => repo.Get(productCreateRequest.CategoryId, cancellationToken), Times.Once);

            _categoryRepositoryMock.Verify(repo => repo.Create(It.IsAny<Category>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Never);
        }
    }
}
