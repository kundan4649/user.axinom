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
    /// <summary>Tests for updating user's watchlist command handler</summary>
    public class UpdateWatchlistCommandHandlerTests
    {
        private readonly Mock<IWatchlistRepository> _watchListRepository = new Mock<IWatchlistRepository>();
        
        /// <summary>Ensure that handler returns success with valid update user's watch list item command</summary>
        [Fact]
        public async Task UpdateWatchlist_Success()
        {
            // Arrange
            var command = new UpdateWatchlistCommand {UserId = Guid.NewGuid()};

            _watchListRepository.Setup(r => r.UpdateItemAsync(It.IsAny<Guid>(), It.IsAny<CatalogItem>()))
                .ReturnsAsync(Result<bool>.FromValue(true));

            var handler = new UpdateWatchlistCommandHandler(_watchListRepository.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Value);
        }
        
        /// <summary>Ensure that handler returns error if updating user's watch list item in repository fails</summary>
        [Fact]
        public async Task UpdateWatchlist_WatchListRepository_Fail()
        {
            // Arrange
            var command = new UpdateWatchlistCommand {UserId = Guid.NewGuid()};
            var error = new Error{Message = "Update item failed"};
            
            _watchListRepository.Setup(r => r.UpdateItemAsync(It.IsAny<Guid>(), It.IsAny<CatalogItem>()))
                .ReturnsAsync(Result<bool>.FromError(error));
            
            var handler = new UpdateWatchlistCommandHandler(_watchListRepository.Object);
            
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(error, result.Error);
            Assert.Null(result.Value);
        }
    }
}