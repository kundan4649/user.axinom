using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using z5.ms.common.abstractions;
using z5.ms.domain.user.settings;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.repositories;
using z5.ms.infrastructure.user.settings;

namespace settings
{
    /// <summary>Tests for delete user's setting command handler</summary>
    public class DeleteSettingCommandHandlerTests
    {
        private readonly Mock<ISettingsRepository> _repository = new Mock<ISettingsRepository>();

        /// <summary>Test that delete setting command handler works with valid input</summary>
        [Fact]
        public async Task DeleteSettingCommand_Success()
        {
            // Arrange
            var command = new DeleteSettingCommand {UserId = Guid.NewGuid()};
            _repository.Setup(r => r.DeleteItemAsync(It.IsAny<Guid>(), It.IsAny<SettingItem>()))
                .ReturnsAsync(Result<bool>.FromValue(true));
            var handler = new DeleteSettingCommandHandler(_repository.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        }
    }
}