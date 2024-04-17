using System;
using System.Collections.Generic;
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
    /// <summary>Tests for fetching user's watchlist query handler</summary>
    public class GetWatchlistQueryHandlerTests
    {
        private readonly Mock<IWatchlistRepository> _watchListRepository = new Mock<IWatchlistRepository>();
        
        /// <summary>Ensure that handler returns success with valid get user's watch list command</summary>
        [Fact]
        public async Task GetWatchlist_Success()
        {
            // Arrange
            var command = new GetWatchlistQuery {UserId = Guid.NewGuid()};

            _watchListRepository.Setup(r => r.GetItemsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(Result<List<CatalogItem>>.FromValue(new List<CatalogItem>()));
            
            var handler = new GetWatchlistQueryHandler(_watchListRepository.Object);
            
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Value);
        }
        
        /// <summary>Ensure that handler returns error if get user's watch list from repository fails</summary>
        [Fact]
        public async Task GetWatchlist_WatchListRepository_Fail()
        {
            // Arrange
            var command = new GetWatchlistQuery {UserId = Guid.NewGuid()};
            var error = new Error{Message = "Get item failed"};
            
            _watchListRepository.Setup(r => r.GetItemsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(Result<List<CatalogItem>>.FromError(error));
            
            var handler = new GetWatchlistQueryHandler(_watchListRepository.Object);
            
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(error, result.Error);
            Assert.Null(result.Value);
        }
    }
}