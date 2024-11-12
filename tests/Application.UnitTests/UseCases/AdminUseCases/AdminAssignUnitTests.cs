using AutoMapper;
using dotnet8_user.Application.UseCases.AdminUseCases.Assign;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using Moq;
using Shouldly;

namespace Application.UnitTests.UseCases.AdminUseCases
{
    public class AdminAssignUnitTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICreateVerifyHash> _createVerifyHashMock;

        public AdminAssignUnitTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _createVerifyHashMock = new Mock<ICreateVerifyHash>();
        }

        [Fact]
        public async Task AssignAdminRole_ValidUser_ReturnsUserResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var assignAdminRoleRequest = new AssignAdminRoleRequest(userId);

            var user = new User(
                email: "user@example.com",
                fullname: "fullname",
                username: "username",
                hashPassword: new byte[32],
                saltPassword: new byte[16]
            );

            var cancellationToken = new CancellationToken();

            _userRepositoryMock.Setup(ur => ur.Get(userId, cancellationToken))
                .ReturnsAsync(user);

            _userRepositoryMock.Setup(ur => ur.AssignRole(user.Id, "Admin", cancellationToken));

            _mapperMock.Setup(m => m.Map<UserResponse>(It.IsAny<User>())).Returns(new UserResponse
            {
                Email = user.Email,
                FullName = user.Fullname,
                UserName = user.UserName
            });

            var handler = new AssignAdminRoleHandler(
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _createVerifyHashMock.Object
            );

            // Act & Assert
            var response = await handler.Handle(assignAdminRoleRequest, cancellationToken);

            response.ShouldNotBeNull();
            response.Email.ShouldBe(user.Email);
            response.FullName.ShouldBe(user.Fullname);
            response.UserName.ShouldBe(user.UserName);

            _userRepositoryMock.Verify(ur => ur.Get(userId, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(ur => ur.AssignRole(user.Id, "Admin", It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task AssignAdminRole_UserNotFound_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var assignAdminRoleRequest = new AssignAdminRoleRequest(userId);

            var cancellationToken = new CancellationToken();

            _userRepositoryMock.Setup(ur => ur.Get(userId, cancellationToken))
                .ReturnsAsync((User)null);

            var handler = new AssignAdminRoleHandler(
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _createVerifyHashMock.Object
            );

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => handler.Handle(assignAdminRoleRequest, cancellationToken));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidOperationException>();
            exception.Message.ShouldBe("User not found.");

            _userRepositoryMock.Verify(ur => ur.Get(userId, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(ur => ur.AssignRole(It.IsAny<Guid>(), "Admin", It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Never);
        }
    }
}
