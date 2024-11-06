using AutoFixture;
using AutoMapper;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Application.UseCases.UserUseCases.Delete;
using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using Moq;
using Shouldly;

namespace Application.UnitTests.UseCases.UserUseCases
{
    public class UserDeleteUnitTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public UserDeleteUnitTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _userRepositoryMock = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task UserExists_DeleteIsCalled_ReturnValidResponseUser()
        {
            // Arrange
            var deleteUserRequest = new Fixture().Create<DeleteUserRequest>();

            byte[] hashPassword = new byte[10];
            byte[] saltPassword = new byte[5];

            var user = new User(
                email: "user@mail.com",
                fullname: "user",
                username: "username",
                hashPassword: hashPassword,
                saltPassword: saltPassword
            );
            var cancellationToken = new CancellationToken();

            _userRepositoryMock
                .Setup(repo => repo.Get(deleteUserRequest.id, cancellationToken))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(repo => repo.Delete(user))
                .Verifiable();

            _mapperMock.Setup(m => m.Map<UserResponse>(user)).Returns(new UserResponse
            {
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.Fullname,
                HashPassword = hashPassword,
                SaltPassword = saltPassword
            });

            var userDeleteService = new DeleteUserHandler(_unitOfWorkMock.Object, _userRepositoryMock.Object, _mapperMock.Object);

            // Act & Assert
            var response = await userDeleteService.Handle(deleteUserRequest, cancellationToken);

            response.ShouldNotBeNull();
            response.Email.ShouldBe(user.Email);
            response.UserName.ShouldBe(user.UserName);
            response.FullName.ShouldBe(user.Fullname);
            response.HashPassword.ShouldBe(hashPassword);
            response.SaltPassword.ShouldBe(saltPassword);

            _userRepositoryMock.Verify(repo => repo.Delete(user), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task UserDoesNotExist_DeleteIsCalled_ReturnDefault()
        {
            // Arrange
            var deleteUserRequest = new Fixture().Create<DeleteUserRequest>();
            var cancellationToken = new CancellationToken();

            _userRepositoryMock
                .Setup(repo => repo.Get(deleteUserRequest.id, cancellationToken))
                .ReturnsAsync((User)null);

            var userDeleteService = new DeleteUserHandler(_unitOfWorkMock.Object, _userRepositoryMock.Object, _mapperMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => userDeleteService.Handle(deleteUserRequest, cancellationToken));
            exception.Message.ShouldBe("User not found.");

            _userRepositoryMock.Verify(repo => repo.Delete(It.IsAny<User>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Never);
        }
    }
}
