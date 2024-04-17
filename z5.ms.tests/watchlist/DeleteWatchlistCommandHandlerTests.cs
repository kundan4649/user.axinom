using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;
using z5.ms.domain.user.watchlist;
using z5.ms.infrastructure.user.repositories;
using z5.ms.infrastructure.user.watchlist;

namespace watchlist
{
    /// <summary>Tests for deleting catalog item from user's watchlist command handler</summary>
    public class DeleteWatchlistCommandHandlerTests
    {
        private readonly Mock<IWatchlistRepository> _watchListRepository = new Mock<IWatchlistRepository>();
        
        /// <summary>Ensure that handler returns success with valid delete item from watch list command</summary>
        [Fact]
        public async Task DeleteWatchlist_Success()
        {
            // Arrange
            var command = new DeleteWatchlistCommand {UserId = Guid.NewGuid(), Item = new CatalogItem()};

            _watchListRepository.Setup(r => r.DeleteItemAsync(It.IsAny<Guid>(), It.IsAny<CatalogItem>()))
                .ReturnsAsync(Result<bool>.FromValue(true));
            
            var handler = new DeleteWatchlistCommandHandler(_watchListRepository.Object);
            
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Value);
        }
        
        /// <summary>Ensure that handler returns error if delete item from watch list repository fails</summary>
        [Fact]
        public async Task DeleteWatchlist_WatchListRepository_Fail()
        {
            // Arrange
            var command = new DeleteWatchlistCommand{UserId = Guid.NewGuid()};
            var error = new Error{Message = "Delete item failed"};
            
            _watchListRepository.Setup(r => r.DeleteItemAsync(It.IsAny<Guid>(), It.IsAny<CatalogItem>()))
                .ReturnsAsync(Result<bool>.FromError(error));
            
            var handler = new DeleteWatchlistCommandHandler(_watchListRepository.Object);
            
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(error, result.Error);
            Assert.Null(result.Value);
        }
    }
}