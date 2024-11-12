using AutoFixture;
using AutoMapper;
using dotnet8_user.Application.UseCases.AdminUseCases.Common;
using dotnet8_user.Application.UseCases.AdminUseCases.Create;
using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using Moq;
using Shouldly;

namespace Application.UnitTests.UseCases.AdminUseCases
{
    public class AdminCreateUnitTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICreateVerifyHash> _createVerifyHashMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public AdminCreateUnitTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _createVerifyHashMock = new Mock<ICreateVerifyHash>();
            _userRepositoryMock = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task ValidAdmin_CreateIsCalled_ReturnValidAdminResponse()
        {
            // Arrange
            var adminCreateRequest = new Fixture().Create<AdminCreateRequest>();

            byte[] hashPassword = new byte[32];
            byte[] saltPassword = new byte[16];

            var admin = new User(
                email: adminCreateRequest.Email,
                fullname: adminCreateRequest.FullName,
                username: adminCreateRequest.UserName,
                hashPassword: hashPassword,
                saltPassword: saltPassword
            );

            _mapperMock.Setup(m => m.Map<User>(adminCreateRequest)).Returns(admin);
            _mapperMock.Setup(m => m.Map<AdminResponse>(It.IsAny<User>())).Returns(new AdminResponse
            {
                Email = adminCreateRequest.Email,
                UserName = adminCreateRequest.UserName,
                FullName = adminCreateRequest.FullName,
                HashPassword = hashPassword,
                SaltPassword = saltPassword
            });

            var handler = new AdminCreateHandler(_userRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _createVerifyHashMock.Object);
            var cancellationToken = new CancellationToken();

            // Act & Assert
            var response = await handler.Handle(adminCreateRequest, cancellationToken);

            response.Email.ShouldBe(adminCreateRequest.Email);
            response.FullName.ShouldBe(adminCreateRequest.FullName);
            response.UserName.ShouldBe(adminCreateRequest.UserName);
            response.HashPassword.ShouldBe(hashPassword);
            response.SaltPassword.ShouldBe(saltPassword);

            _userRepositoryMock.Verify(er => er.CreateAdmin(
                It.Is<User>(u => u.Email == adminCreateRequest.Email),
                It.Is<List<string>>(roles => roles.Contains("Admin")),
                It.IsAny<CancellationToken>()
            ), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task InvalidAdmin_CreateFails_ReturnErrorResponse()
        {
            // Arrange
            var adminCreateRequest = new Fixture().Create<AdminCreateRequest>();

            _userRepositoryMock.Setup(er => er.CreateAdmin(It.IsAny<User>(), It.IsAny<List<string>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Invalid admin creation"));

            var handler = new AdminCreateHandler(_userRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _createVerifyHashMock.Object);
            var cancellationToken = new CancellationToken();

            // Act && Assert
            var exception = await Record.ExceptionAsync(() => handler.Handle(adminCreateRequest, cancellationToken));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidOperationException>();
            exception.Message.ShouldBe("Invalid admin creation");

            _userRepositoryMock.Verify(er => er.CreateAdmin(It.IsAny<User>(), It.IsAny<List<string>>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Never);
        }
    }
}
