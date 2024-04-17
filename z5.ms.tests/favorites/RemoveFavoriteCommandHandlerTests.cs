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
    /// <summary>Tests for remove user's favorite item command handler</summary>
    public class RemoveFavoriteCommandHandlerTests
    {
        private readonly Mock<IFavoritesRepository> _repository = new Mock<IFavoritesRepository>();
        
        /// <inheritdoc />
        public RemoveFavoriteCommandHandlerTests()
        {
        }

        /// <summary>Test that get favorites query handler works with valid input</summary>
        [Fact]
        public async Task RemoveFavoriteCommand_Success()
        {
            // Arrange
            var command = new RemoveFavoriteCommand {UserId = Guid.NewGuid()};
            _repository.Setup(r => r.DeleteItemAsync(It.IsAny<Guid>(), It.IsAny<CatalogItem>()))
                .ReturnsAsync(Result<bool>.FromValue(true));
            var handler = new RemoveFavoriteCommandHandler(_repository.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        } 
    }
}