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
    public class ForgotPasswordMobileCommandHandlerTests
    {
        private readonly Mock<IPasswordService> _passwordService = new Mock<IPasswordService>();
        private readonly ForgotPasswordMobileCommandHandler _handler;
        
        public ForgotPasswordMobileCommandHandlerTests()
        {
            _handler = new ForgotPasswordMobileCommandHandler(_passwordService.Object);
        }

        /// <summary>Ensure that handler returns success with valid forgot user password command</summary>
        [Fact]
        public async Task ForgotPasswordMobileCommand_Success()
        {
            // Arrange
            var command = new ForgotPasswordMobileCommand();

            _passwordService.Setup(r => r.SendPasswordResetNotification(It.IsAny<ForgotPasswordCommand>()))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));

            // Act
            var result = await _handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        }

        /// <summary>Ensure that handler returns Error if generating user's code fails for some reason</summary>
        [Fact]
        public async Task ForgotPasswordMobileCommand_Fail()
        {
            // Arrange
            var command = new ForgotPasswordMobileCommand();

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