using System;
using System.Collections.Generic;
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
    /// <summary>Tests for get user settings query handler</summary>
    public class GetSettingsQueryHandlerTests
    {
        private readonly Mock<ISettingsRepository> _repository = new Mock<ISettingsRepository>();
        
        /// <inheritdoc />
        public GetSettingsQueryHandlerTests()
        {
        }

        /// <summary>Test that get settings query handler works with valid input</summary>
        [Fact]
        public async Task GetSettingsCommand_Success()
        {
            // Arrange
            var command = new GetSettingsQuery {UserId = Guid.NewGuid()};
            _repository.Setup(r => r.GetItemsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(Result<List<SettingItem>>.FromValue(new List<SettingItem>()));
            var handler = new GetSettingsQueryHandler(_repository.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        }
    }
}