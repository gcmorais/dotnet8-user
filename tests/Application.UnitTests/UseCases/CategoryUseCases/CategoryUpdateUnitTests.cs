using AutoMapper;
using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Application.UseCases.UserUseCases.Update;
using dotnet8_user.Domain.Entities;
using dotnet8_user.Domain.Interfaces;
using FluentValidation;
using Moq;
using Shouldly;

namespace Application.UnitTests.UseCases.UserUseCases
{
    public class CategoryUpdateUnitTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidator<UserUpdateRequest>> _validatorMock;

        public CategoryUpdateUnitTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _validatorMock = new Mock<IValidator<UserUpdateRequest>>();
        }

        [Fact]
        public async Task ValidUser_UpdateIsCalled_ReturnValidUserResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UserUpdateRequest
            {
                Id = userId,
                FullName = "Updated Full Name",
                UserName = "UpdatedUsername",
                Email = "updated@example.com"
            };

            byte[] hashPassword = new byte[32];
            byte[] saltPassword = new byte[16];

            var user = new User("FullName", "Username", "user@example.com", hashPassword, saltPassword)
            {
                Id = userId
            };

            var userResponse = new UserResponse
            {
                Id = userId,
                FullName = "Updated Full Name",
                UserName = "UpdatedUsername",
                Email = "updated@example.com"
            };

            _userRepositoryMock.Setup(repo => repo.Get(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _mapperMock.Setup(mapper => mapper.Map<UserResponse>(user))
                .Returns(userResponse);

            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var handler = new UserUpdateHandler(_unitOfWorkMock.Object, _userRepositoryMock.Object, _mapperMock.Object, _validatorMock.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldBe(userResponse);

            _userRepositoryMock.Verify(repo => repo.Get(userId, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<UserResponse>(user), Times.Once);
        }

        [Fact]
        public async Task InvalidUser_UpdateThrowsValidationException()
        {
            // Arrange
            var request = new UserUpdateRequest
            {
                Id = Guid.NewGuid(),
                FullName = "",
                UserName = "",
                Email = ""
            };

            var validationResult = new FluentValidation.Results.ValidationResult(new[]
            {
                new FluentValidation.Results.ValidationFailure("FullName", "Full name is required."),
                new FluentValidation.Results.ValidationFailure("UserName", "User name is required."),
                new FluentValidation.Results.ValidationFailure("Email", "Email is required.")
            });

            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var handler = new UserUpdateHandler(_unitOfWorkMock.Object, _userRepositoryMock.Object, _mapperMock.Object, _validatorMock.Object);

            // Act & Assert
            await Should.ThrowAsync<FluentValidation.ValidationException>(async () =>
                await handler.Handle(request, CancellationToken.None));

            _validatorMock.Verify(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
