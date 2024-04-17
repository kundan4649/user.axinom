using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;
using z5.ms.domain.user.watchhistory;
using z5.ms.infrastructure.user.repositories;
using z5.ms.infrastructure.user.watchhistory;

namespace watchhistory
{
    /// <summary>Tests for updating a catalog item in user's watch history command handler</summary>
    public class UpdateWatchhistoryCommandHandlerTests
    {
        private readonly Mock<IWatchHistoryRepository> _watchHistoryRepository = new Mock<IWatchHistoryRepository>();
        
        /// <summary>Ensure that handler returns success with valid update watch history item command</summary>
        [Fact]
        public async Task UpdateWatchhistory_Success()
        {
            // Arrange
            var command = new UpdateWatchhistoryCommand {UserId = Guid.NewGuid(), Item = new CatalogItem()};

            _watchHistoryRepository.Setup(r => r.UpdateItemAsync(It.IsAny<Guid>(), It.IsAny<CatalogItem>()))
                .ReturnsAsync(Result<bool>.FromValue(true));
            
            var handler = new UpdateWatchhistoryCommandHandler(_watchHistoryRepository.Object);
            
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Value);
        }
        
        /// <summary>Ensure that handler returns error if update item from watch history repository fails</summary>
        [Fact]
        public async Task UpdateWatchhistory_WatchHistoryRepository_Fail()
        {
            // Arrange
            var command = new UpdateWatchhistoryCommand{UserId = Guid.NewGuid()};
            var error = new Error{Message = "Update item failed"};
            
            _watchHistoryRepository.Setup(r => r.UpdateItemAsync(It.IsAny<Guid>(), It.IsAny<CatalogItem>()))
                .ReturnsAsync(Result<bool>.FromError(error));
            
            var handler = new UpdateWatchhistoryCommandHandler(_watchHistoryRepository.Object);
            
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(error, result.Error);
            Assert.Null(result.Value);
        }
    }
}