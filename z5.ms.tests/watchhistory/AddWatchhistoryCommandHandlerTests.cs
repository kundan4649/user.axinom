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
    public class AddWatchhistoryCommandHandlerTests
    {
        private readonly Mock<IWatchHistoryRepository> _watchHistoryRepository = new Mock<IWatchHistoryRepository>();
        
        /// <summary>Ensure that handler returns success with valid add item to watch history command</summary>
        [Fact]
        public async Task AddWatchhistory_Success()
        {
            // Arrange
            var command = new AddWatchhistoryCommand {UserId = Guid.NewGuid(), Item = new CatalogItem()};

            _watchHistoryRepository.Setup(r => r.AddItemAsync(It.IsAny<Guid>(), It.IsAny<CatalogItem>()))
                .ReturnsAsync(Result<bool>.FromValue(true));
            
            var handler = new AddWatchhistoryCommandHandler(_watchHistoryRepository.Object);
            
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Value);
        }
        
        /// <summary>Ensure that handler returns error if add item to watch history repository fails</summary>
        [Fact]
        public async Task AddWatchhistory_WatchHistoryRepository_Fail()
        {
            // Arrange
            var command = new AddWatchhistoryCommand{UserId = Guid.NewGuid()};
            var error = new Error{Message = "Add item failed"};
            
            _watchHistoryRepository.Setup(r => r.AddItemAsync(It.IsAny<Guid>(), It.IsAny<CatalogItem>()))
                .ReturnsAsync(Result<bool>.FromError(error));
            
            var handler = new AddWatchhistoryCommandHandler(_watchHistoryRepository.Object);
            
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(error, result.Error);
            Assert.Null(result.Value);
        }
    }
}