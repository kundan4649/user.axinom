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
    /// <summary>Tests for get user's watch history filtered by asset id query handler</summary>
    public class GetWatchhistoryByIdQueryHandlerTests
    {
        private readonly Mock<IWatchHistoryRepository> _watchHistoryRepository = new Mock<IWatchHistoryRepository>();
        
        /// <summary>Ensure that handler returns success with valid get watch history item by id command</summary>
        [Fact]
        public async Task GetWatchhistoryById_Success()
        {
            // Arrange
            var command = new GetWatchhistoryByIdQuery {UserId = Guid.NewGuid()};

            _watchHistoryRepository.Setup(r => r.GetItemsByAssetIdAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(Result<CatalogItem>.FromValue(new CatalogItem()));
            
            var handler = new GetWatchhistoryByIdQueryHandler(_watchHistoryRepository.Object);
            
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Value);
        }
        
        /// <summary>Ensure that handler returns error if get item by id from watch history repository fails</summary>
        [Fact]
        public async Task GetWatchhistoryById_WatchHistoryRepository_Fail()
        {
            // Arrange
            var command = new GetWatchhistoryByIdQuery {UserId = Guid.NewGuid()};
            var error = new Error{Message = "Get item failed"};
            
            _watchHistoryRepository.Setup(r => r.GetItemsByAssetIdAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(Result<CatalogItem>.FromError(error));

            var handler = new GetWatchhistoryByIdQueryHandler(_watchHistoryRepository.Object);
            
            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(error, result.Error);
            Assert.Null(result.Value);
        }
    }
}