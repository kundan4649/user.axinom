using System;
using System.Collections.Generic;
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
    /// <summary>Tests for get user favorites query handler</summary>
    public class GetFavoritesQueryHandlerTests
    {
        private readonly Mock<IFavoritesRepository> _repository = new Mock<IFavoritesRepository>();
        
        /// <inheritdoc />
        public GetFavoritesQueryHandlerTests()
        {
        }

        /// <summary>Test that get favorites query handler works with valid input</summary>
        [Fact]
        public async Task GetFavoritesCommand_Success()
        {
            // Arrange
            var command = new GetFavoritesQuery {UserId = Guid.NewGuid()};
            _repository.Setup(r => r.GetItemsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(Result<List<CatalogItem>>.FromValue(new List<CatalogItem>()));
            var handler = new GetFavoritesQueryHandler(_repository.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        }
    }
}