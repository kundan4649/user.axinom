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
    /// <summary>Tests for adding favorite item to user command handler</summary>
    public class AddFavoriteCommandHandlerTests
    {
        private readonly Mock<IFavoritesRepository> _repository = new Mock<IFavoritesRepository>();
        
        /// <inheritdoc />
        public AddFavoriteCommandHandlerTests()
        {
        }

        /// <summary>Test that add favorite command handler works with valid input</summary>
        [Fact]
        public async Task AddFavoriteCommand_Success()
        {
            // Arrange
            var command = new AddFavoriteCommand {UserId = Guid.NewGuid()};
            _repository.Setup(r => r.AddItemAsync(It.IsAny<Guid>(), It.IsAny<CatalogItem>()))
                .ReturnsAsync(Result<bool>.FromValue(true));
            var handler = new AddFavoriteCommandHandler(_repository.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        }
    }
}