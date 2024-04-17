using System.Threading.Tasks;
using Moq;
using Xunit;
using z5.ms.common.abstractions;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.services;
using z5.ms.infrastructure.user.user;

namespace user
{
    /// <summary>Tests for confirming user email or mobile command handler</summary>
    public class ConfirmUserCommandHandlerTests
    {
        private readonly Mock<IConfirmationService> _confirmationService = new Mock<IConfirmationService>();

        /// <summary>Ensure that handler returns success with confirm user's email command</summary>
        [Fact]
        public async Task ConfirmUserCommand_Success()
        {
            // Arrange
            var command = new ConfirmUserCommand();

            _confirmationService.Setup(r => r.Confirm(It.IsAny<ConfirmUserCommand>()))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));

            var handler = new ConfirmUserCommandHandler(_confirmationService.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        }

        /// <summary>Ensure that handler returns error with confirm user's email command</summary>
        [Fact]
        public async Task ChangePasswordCommand_Fail()
        {
            // Arrange
            var command = new ConfirmUserCommand();
            var error = new Error { Code = 123, Message = "error" };

            _confirmationService.Setup(r => r.Confirm(It.IsAny<ConfirmUserCommand>()))
                .ReturnsAsync(Result<Success>.FromError(error));

            var handler = new ConfirmUserCommandHandler(_confirmationService.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(error, result.Error);
        }
    }
}