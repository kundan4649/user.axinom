using System.Threading.Tasks;
using Moq;
using Xunit;
using z5.ms.common.abstractions;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.services;
using z5.ms.infrastructure.user.user;

namespace user
{
    /// <summary>Tests for recreating user password with received code command handler</summary>
    public class ResetPasswordCommandHandlerTests
    {
        private readonly Mock<IPasswordService> _passwordService = new Mock<IPasswordService>();
        
        /// <summary>Ensure that handler return success with valid recreate user password command</summary>
        [Fact]
        public async Task ResetPasswordCommand_Success()
        {
            // Arrange
            var command = new ResetPasswordCommand();

            _passwordService.Setup(r => r.ResetPassword(It.IsAny<ResetPasswordCommand>()))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));

            var handler = new ResetPasswordCommandHandler(_passwordService.Object);
            
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        }
        
        /// <summary>Ensure that handler return error when reading user by mobile fails from user repository</summary>
        [Fact]
        public async Task ResetPasswordCommand_Fail()
        {
            // Arrange
            var command = new ResetPasswordCommand();
            var error = new Error {Code = 123, Message = "error"};

            _passwordService.Setup(r => r.ResetPassword(It.IsAny<ResetPasswordCommand>()))
                .ReturnsAsync(Result<Success>.FromError(error));

            var handler = new ResetPasswordCommandHandler(_passwordService.Object);
            
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(error, result.Error);
        }
    }
}