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
    /// <summary>Tests for updating user's setting item</summary>
    public class UpdateSettingCommandHandlerTests
    {
        private readonly Mock<ISettingsRepository> _repository = new Mock<ISettingsRepository>();

        /// <summary>Test that update setting command handler works with valid input</summary>
        [Fact]
        public async Task UpdateSettingCommand_Success()
        {
            // Arrange
            var command = new UpdateSettingCommand {UserId = Guid.NewGuid(), Item = new SettingItem()};
            _repository.Setup(r => r.UpdateItemAsync(It.IsAny<Guid>(), It.IsAny<SettingItem>()))
                .ReturnsAsync(Result<bool>.FromValue(true));
            var handler = new UpdateSettingCommandHandler(_repository.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        }
    }
}