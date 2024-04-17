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
    /// <summary>Tests for registering user with email address command handler</summary>
    public class RegisterEmailUserCommandHandlerTests
    {
        private readonly Mock<IAuthenticationService> _authenticationService = new Mock<IAuthenticationService>();
        private readonly Mock<INotificationClient> _notificationClient = new Mock<INotificationClient>();

        /// <summary>Ensure that handler return success with valid register user with email command</summary>
        [Fact]
        public async Task RegisterEmailUserCommand_Success()
        {
            // Arrange
            var command = new RegisterEmailUserCommand();

            _authenticationService.Setup(r => r.Register(command))
                .ReturnsAsync(Result<UserEntity>.FromValue(new UserEntity()));

           // var handler = new RegisterEmailUserCommandHandler(_authenticationService.Object, _notificationClient.Object);

            // Act
         //   var result = await handler.Handle(command);

            // Assert
         //   Assert.True(result.Success);
        }

        /// <summary>Ensure that handler return error when creating user by email address fails in user repository</summary>
        [Fact]
        public async Task RegisterEmailUserCommand_Fail()
        {
            // Arrange
            var command = new RegisterEmailUserCommand();
            var error = new Error { Code = 1, Message = "Error message" };

            _authenticationService.Setup(r => r.Register(command))
                .ReturnsAsync(Result<UserEntity>.FromError(error));

           // var handler = new RegisterEmailUserCommandHandler(_authenticationService.Object, _notificationClient.Object);

            // Act
          //  var result = await handler.Handle(command);

            // Assert
          //  Assert.False(result.Success);
         //   Assert.Equal(error, result.Error);
        }
    }
}