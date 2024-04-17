using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using z5.ms.common.abstractions;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.services;
using z5.ms.infrastructure.user.user;

namespace user
{
    /// <summary>Tests for changing user's password command handler</summary>
    public class ChangePasswordCommandHandlerTests
    {
        private readonly Mock<IPasswordService> _passwordService = new Mock<IPasswordService>();
        
        /// <summary>Ensure that handler returns success with update user's password command</summary>
        [Fact]
        public async Task ChangePasswordCommand_Success()
        {
            // Arrange
            var command = new ChangePasswordCommand  {UserId = Guid.NewGuid(), NewPassword = "newpass", OldPassword = "oldpass" };
            
            _passwordService.Setup(r => r.ChangePassword(It.IsAny<ChangePasswordCommand>()))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));
            
            var handler = new ChangePasswordCommandHandler(_passwordService.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        }

        /// <summary>Ensure that handler returns error with update user's password command</summary>
        [Fact]
        public async Task ChangePasswordCommand_Fail()
        {
            // Arrange
            var command = new ChangePasswordCommand { UserId = Guid.NewGuid(), NewPassword = "newpass", OldPassword = "oldpass" };
            var error = new Error { Code = 123, Message = "error" };

            _passwordService.Setup(r => r.ChangePassword(It.IsAny<ChangePasswordCommand>()))
                .ReturnsAsync(Result<Success>.FromError(error));

            var handler = new ChangePasswordCommandHandler(_passwordService.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(error, result.Error);
        }
    }
}