using System.Threading.Tasks;
using Moq;
using Xunit;
using z5.ms.common.abstractions;
using z5.ms.common.notifications;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.services;
using z5.ms.infrastructure.user.user;

namespace user
{
    /// <summary>Tests for resending confirmation code to user command handler</summary>
    public class ResendConfirmationEmailCommandHandlerTests
    {
        private readonly Mock<IConfirmationService> _confirmationService = new Mock<IConfirmationService>();
        private readonly Mock<INotificationClient> _notificationClient = new Mock<INotificationClient>();

        /// <summary>Ensure that handler return success with valid existing user command</summary>
        [Fact]
        public async Task ResendConfirmationEmailCommand_Success()
        {
            // Arrange
            var command = new ResendConfirmationEmailCommand();

            _confirmationService.Setup(r => r.ReCreateConfirmationCode(It.IsAny<ResendConfirmationEmailCommand>()))
                .ReturnsAsync(Result<UserEntity>.FromValue(new UserEntity()));

            _notificationClient.Setup(r => r.SendRegistrationActivationEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            
            var handler = new ResendConfirmationEmailCommandHandler(_confirmationService.Object, _notificationClient.Object);
            
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Value);
        }
        
        /// <summary>Ensure that handler returns error when fetching user from user repository fails </summary>
        [Fact]
        public async Task ResendConfirmationEmailCommand_Fail()
        {
            // Arrange
            var command = new ResendConfirmationEmailCommand();
            var error = new Error { Code = 123, Message = "error" };

            _confirmationService.Setup(r => r.ReCreateConfirmationCode(It.IsAny<ResendConfirmationEmailCommand>()))
                .ReturnsAsync(Result<UserEntity>.FromError(error));
            
            var handler = new ResendConfirmationEmailCommandHandler(_confirmationService.Object, _notificationClient.Object);
            
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(error, result.Error);
        }
    }
}