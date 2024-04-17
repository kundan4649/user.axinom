using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using z5.ms.common.abstractions;
using z5.ms.domain.user.favorites;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.favorites;
using z5.ms.infrastructure.user.repositories;

namespace favorites
{
    /// <summary>Tests for updating user's favorite catalog item</summary>
    public class UpdateFavoritesCommandHandlerTests
    {
        private readonly Mock<IFavoritesRepository> _repository = new Mock<IFavoritesRepository>();
        
        /// <inheritdoc />
        public UpdateFavoritesCommandHandlerTests()
        {
        }

        /// <summary>Test that update favorite command handler works with valid input</summary>
        [Fact]
        public async Task UpdateFavoriteCommand_Success()
        {
            // Arrange
            var command = new UpdateFavoriteCommand {UserId = Guid.NewGuid(), Item = new CatalogItem()};
            _repository.Setup(r => r.UpdateItemAsync(It.IsAny<Guid>(), It.IsAny<CatalogItem>()))
                .ReturnsAsync(Result<bool>.FromValue(true));
            var handler = new UpdateFavoriteCommandHandler(_repository.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        } 
    }
}