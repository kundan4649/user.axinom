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
    /// <summary>Tests for adding a new setting to user command</summary>
    public class AddSettingCommandHandlerTests
    {
        private readonly Mock<ISettingsRepository> _repository = new Mock<ISettingsRepository>();

        /// <summary>Test that add setting command handler works with valid input</summary>
        [Fact]
        public async Task AddSettingCommand_Success()
        {
            // Arrange
            var command = new AddSettingCommand {UserId = Guid.NewGuid()};
            _repository.Setup(r => r.AddItemAsync(It.IsAny<Guid>(), It.IsAny<SettingItem>()))
                .ReturnsAsync(Result<bool>.FromValue(true));
            var handler = new AddSettingCommandHandler(_repository.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        }
    }
}