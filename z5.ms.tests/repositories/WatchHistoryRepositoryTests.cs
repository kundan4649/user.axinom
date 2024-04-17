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
//    public class WatchHistoryRepositoryTests
//    {
//        [Fact]
//        public async void GetItemsAsync_ReturnsWatchHistory()
//        {
//            // Arrange
//            var userId = new WatchHistoryMocks().WatchHistoryEntities.FirstOrDefault()?.UserId;
//            var watchHistoryEntities = new WatchHistoryMocks().WatchHistoryEntities.Where(a => a.UserId == userId).ToList();

//            var mockDbService = new Mock<IDbService>();
//            mockDbService.Setup(db => db.GetItemsWhere<WatchHistoryEntity>(nameof(WatchHistoryEntity.UserId), userId)).ReturnsAsync(watchHistoryEntities);
//            var mockMapper = new Mock<IMapper>();
//            mockMapper.Setup(map => map.Map<WatchHistoryEntity, CatalogItem>(It.IsAny<WatchHistoryEntity>())).Returns(new WatchHistoryMocks().WatchHistory.FirstOrDefault());

//            var repo = new WatchHistoryRepository(mockMapper.Object, Options.Create(new UserErrors()), Options.Create(new DbConnectionOptions()));

//            // Act
//            var result = await repo.GetItemsAsync((Guid)userId);

//            // Assert
//            Assert.True(result.Success);
//            var subscriptionsResult = Assert.IsType<List<CatalogItem>>(result.Value);
//            Assert.NotNull(subscriptionsResult);
//            Assert.Equal(subscriptionsResult.FirstOrDefault()?.AssetId, watchHistoryEntities.FirstOrDefault()?.AssetId);
//        }

//        [Fact]
//        public async void AddItemAsync_ReturnsSuccessResult()
//        {
//            // Arrange
//            var watchHistoryEntity = new WatchHistoryMocks().WatchHistoryEntities.FirstOrDefault();

//            var mockDbService = new Mock<IDbService>();
//            mockDbService.Setup(db => db.InsertItem<WatchHistoryEntity>(It.IsAny<WatchHistoryEntity>())).ReturnsAsync(true);
//            var mockMapper = new Mock<IMapper>();
//            mockMapper.Setup(map => map.Map<CatalogItem, WatchHistoryEntity> (It.IsAny<CatalogItem>())).Returns(watchHistoryEntity);

//            var repo = new WatchHistoryRepository(mockMapper.Object, Options.Create(new UserErrors()), Options.Create(new DbConnectionOptions()));

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
//            var userId = new WatchHistoryMocks().WatchHistoryEntities.FirstOrDefault()?.UserId;
//            var watchHistoryEntities = new WatchHistoryMocks().WatchHistoryEntities.Where(a => a.UserId == userId).ToList();
//            var item = new WatchHistoryMocks().WatchHistory.FirstOrDefault();

//            var mockDbService = new Mock<IDbService>();
//            mockDbService.Setup(db => db.GetItemsWhere<WatchHistoryEntity>(nameof(WatchHistoryEntity.UserId), userId)).ReturnsAsync(watchHistoryEntities);
//            mockDbService.Setup(db => db.DeleteItem<WatchHistoryEntity>(It.IsAny<WatchHistoryEntity>())).ReturnsAsync(true);
//            var mockMapper = new Mock<IMapper>();

//            var repo = new WatchHistoryRepository(mockMapper.Object, Options.Create(new UserErrors()), Options.Create(new DbConnectionOptions()));

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
//            var userId = new WatchHistoryMocks().WatchHistoryEntities.FirstOrDefault()?.UserId;
//            var watchHistoryEntities = new WatchHistoryMocks().WatchHistoryEntities.Where(a => a.UserId == userId).ToList();
//            var item = new WatchHistoryMocks().WatchHistory.FirstOrDefault();

//            var mockDbService = new Mock<IDbService>();
//            mockDbService.Setup(db => db.GetItemsWhere<WatchHistoryEntity>(nameof(WatchHistoryEntity.UserId), userId)).ReturnsAsync(watchHistoryEntities);
//            mockDbService.Setup(db => db.UpdateItem<WatchHistoryEntity>(It.IsAny<WatchHistoryEntity>())).ReturnsAsync(true);
//            var mockMapper = new Mock<IMapper>();

//            var repo = new WatchHistoryRepository(mockMapper.Object, Options.Create(new UserErrors()), Options.Create(new DbConnectionOptions()));

//            // Act
//            var result = await repo.UpdateItemAsync((Guid)userId, item);

//            // Assert
//            Assert.True(result.Success);
//            var boolResult = Assert.IsType<bool>(result.Value);
//            Assert.True(boolResult);
//        }
//    }
//}
