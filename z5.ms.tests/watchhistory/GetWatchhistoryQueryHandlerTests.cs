using System;
using System.Collections.Generic;
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
    /// <summary>Tests for get user's watch history query handler</summary>
    public class GetWatchhistoryQueryHandlerTests
    {
        private readonly Mock<IWatchHistoryRepository> _watchHistoryRepository = new Mock<IWatchHistoryRepository>();
        
        /// <summary>Ensure that handler returns success with valid get watch history items command</summary>
        [Fact]
        public async Task GetWatchhistory_Success()
        {
            // Arrange
            var command = new GetWatchhistoryQuery {UserId = Guid.NewGuid()};

            _watchHistoryRepository.Setup(r => r.GetItemsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(Result<List<CatalogItem>>.FromValue(new List<CatalogItem>()));
            
            var handler = new GetWatchhistoryQueryHandler(_watchHistoryRepository.Object);
            
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Value);
        }
        
        /// <summary>Ensure that handler returns error if get item from watch history repository fails</summary>
        [Fact]
        public async Task GetWatchhistory_WatchHistoryRepository_Fail()
        {
            // Arrange
            var command = new GetWatchhistoryQuery {UserId = Guid.NewGuid()};
            var error = new Error{Message = "Get item failed"};
            
            _watchHistoryRepository.Setup(r => r.GetItemsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(Result<List<CatalogItem>>.FromError(error));

            var handler = new GetWatchhistoryQueryHandler(_watchHistoryRepository.Object);
            
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(error, result.Error);
            Assert.Null(result.Value);
        }
    }
}