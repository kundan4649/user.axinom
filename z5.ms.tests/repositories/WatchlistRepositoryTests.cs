//using System;
//using System.Collections.Generic;
//using System.Linq;
//using AutoMapper;
//using Microsoft.Extensions.Options;
//using Moq;
//using Xunit;
//using z5.ms.common.infrastructure.db;
//using z5.ms.domain.user;
//using z5.ms.domain.user.datamodels;
//using z5.ms.domain.user.viewmodels;
//using z5.ms.infrastructure.user.mocks;
//using z5.ms.infrastructure.user.repositories;
//using z5.ms.infrastructure.user.services;

//namespace repositories
//{
//    public class WatchlistRepositoryTests
//    {
//        [Fact]
//        public async void GetItemsAsync_ReturnsWatchlist()
//        {
//            // Arrange
//            var userId = new WatchlistMocks().WatchlistEntities.FirstOrDefault()?.UserId;
//            var watchlistEntities = new WatchlistMocks().WatchlistEntities.Where(a => a.UserId == userId).ToList();

//            var mockDbService = new Mock<IDbService>();
//            mockDbService.Setup(db => db.GetItemsWhere<WatchlistEntity>(nameof(WatchlistEntity.UserId), userId)).ReturnsAsync(watchlistEntities);
//            var mockMapper = new Mock<IMapper>();
//            mockMapper.Setup(map => map.Map<WatchlistEntity, CatalogItem>(It.IsAny<WatchlistEntity>())).Returns(new WatchlistMocks().Watchlist.FirstOrDefault());

//            var repo = new WatchlistRepository(mockMapper.Object, Options.Create(new UserErrors()), Options.Create(new DbConnectionOptions()));

//            // Act
//            var result = await repo.GetItemsAsync((Guid)userId);

//            // Assert
//            Assert.True(result.Success);
//            var subscriptionsResult = Assert.IsType<List<CatalogItem>>(result.Value);
//            Assert.NotNull(subscriptionsResult);
//            Assert.Equal(subscriptionsResult.FirstOrDefault()?.AssetId, watchlistEntities.FirstOrDefault()?.AssetId);
//        }

//        [Fact]
//        public async void AddItemAsync_ReturnsSuccessResult()
//        {
//            // Arrange
//            var watchlistEntity = new WatchlistMocks().WatchlistEntities.FirstOrDefault();

//            var mockDbService = new Mock<IDbService>();
//            mockDbService.Setup(db => db.InsertItem<WatchlistEntity>(It.IsAny<WatchlistEntity>())).ReturnsAsync(true);
//            var mockMapper = new Mock<IMapper>();
//            mockMapper.Setup(map => map.Map<CatalogItem, WatchlistEntity> (It.IsAny<CatalogItem>())).Returns(watchlistEntity);

//            var repo = new WatchlistRepository(mockMapper.Object, Options.Create(new UserErrors()), Options.Create(new DbConnectionOptions()));

//            // Act
//            var result = await repo.AddItemAsync(Guid.NewGuid(), new CatalogItem());

//            // Assert
//            Assert.True(result.Success);
//            var boolResult = Assert.IsType<bool>(result.Value);
//            Assert.True(boolResult);
//        }

//        [Fact]
//        public async void DeleteItemAsync_ReturnsSuccessResult()
//        {
//            // Arrange
//            var userId = new WatchlistMocks().WatchlistEntities.FirstOrDefault()?.UserId;
//            var watchlistEntities = new WatchlistMocks().WatchlistEntities.Where(a => a.UserId == userId).ToList();
//            var item = new WatchlistMocks().Watchlist.FirstOrDefault();

//            var mockDbService = new Mock<IDbService>();
//            mockDbService.Setup(db => db.GetItemsWhere<WatchlistEntity>(nameof(WatchlistEntity.UserId), userId)).ReturnsAsync(watchlistEntities);
//            mockDbService.Setup(db => db.DeleteItem<WatchlistEntity>(It.IsAny<WatchlistEntity>())).ReturnsAsync(true);
//            var mockMapper = new Mock<IMapper>();

//            var repo = new WatchlistRepository(mockMapper.Object, Options.Create(new UserErrors()), Options.Create(new DbConnectionOptions()));

//            // Act
//            var result = await repo.DeleteItemAsync((Guid)userId, item);

//            // Assert
//            Assert.True(result.Success);
//            var boolResult = Assert.IsType<bool>(result.Value);
//            Assert.True(boolResult);
//        }

//        [Fact]
//        public async void UpdateItemAsync_ReturnsSuccessResult()
//        {
//            // Arrange
//            var userId = new WatchlistMocks().WatchlistEntities.FirstOrDefault()?.UserId;
//            var watchlistEntities = new WatchlistMocks().WatchlistEntities.Where(a => a.UserId == userId).ToList();
//            var item = new WatchlistMocks().Watchlist.FirstOrDefault();

//            var mockDbService = new Mock<IDbService>();
//            mockDbService.Setup(db => db.GetItemsWhere<WatchlistEntity>(nameof(WatchlistEntity.UserId), userId)).ReturnsAsync(watchlistEntities);
//            mockDbService.Setup(db => db.UpdateItem<WatchlistEntity>(It.IsAny<WatchlistEntity>())).ReturnsAsync(true);
//            var mockMapper = new Mock<IMapper>();

//            var repo = new WatchlistRepository(mockMapper.Object, Options.Create(new UserErrors()), Options.Create(new DbConnectionOptions()));

//            // Act
//            var result = await repo.UpdateItemAsync((Guid)userId, item);

//            // Assert
//            Assert.True(result.Success);
//            var boolResult = Assert.IsType<bool>(result.Value);
//            Assert.True(boolResult);
//        }
//    }
//}
