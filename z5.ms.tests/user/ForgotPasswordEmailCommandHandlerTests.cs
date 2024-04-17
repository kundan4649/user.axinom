using System.Threading.Tasks;
using Moq;
using Xunit;
using z5.ms.common.abstractions;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.services;
using z5.ms.infrastructure.user.user;

namespace user
{
    /// <summary>Tests for forgot user password command</summary>
    public class ForgotPasswordEmailCommandHandlerTests
    {
        private readonly Mock<IPasswordService> _passwordService = new Mock<IPasswordService>();
        private readonly ForgotPasswordEmailCommandHandler _handler;
        
        public ForgotPasswordEmailCommandHandlerTests()
        {
            _handler = new ForgotPasswordEmailCommandHandler(_passwordService.Object);
        }

        /// <summary>Ensure that handler returns success with valid forgot user password command</summary>
        [Fact]
        public async Task ForgotPasswordEmailCommand_Success()
        {
            // Arrange
            var command = new ForgotPasswordEmailCommand();

            _passwordService.Setup(r => r.SendPasswordResetNotification(It.IsAny<ForgotPasswordCommand>()))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));

            // Act
            var result = await _handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        }
        
        /// <summary>Ensure that handler returns Error if generating user's code fails for some reason</summary>
        [Fact]
        public async Task ForgotPasswordEmailCommand_Fail()
        {
            // Arrange
            var command = new ForgotPasswordEmailCommand();

            var error = new Error { Code = 111, Message = "error" };

            _passwordService.Setup(r => r.SendPasswordResetNotification(It.IsAny<ForgotPasswordCommand>()))
                .ReturnsAsync(Result<Success>.FromError(error));
            
            // Act
            var result = await _handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(error, result.Error);
        }
    }
}