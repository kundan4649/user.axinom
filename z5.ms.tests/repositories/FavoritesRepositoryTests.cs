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
//    public class FavoritesRepositoryTests
//    {
//        [Fact]
//        public async void GetItemsAsync_ReturnsFavorites()
//        {
//            // Arrange
//            var userId = new FavoritesMocks().FavoritesEntities.FirstOrDefault()?.UserId;
//            var favoriteEntities = new FavoritesMocks().FavoritesEntities.Where(a => a.UserId == userId).ToList();

//            var mockDbService = new Mock<IDbService>();
//            mockDbService.Setup(db => db.GetItemsWhere<FavoriteEntity>(nameof(FavoriteEntity.UserId), userId)).ReturnsAsync(favoriteEntities);
//            var mockMapper = new Mock<IMapper>();
//            mockMapper.Setup(map => map.Map<FavoriteEntity, CatalogItem>(It.IsAny<FavoriteEntity>())).Returns(new FavoritesMocks().Favorites.FirstOrDefault());

//            var repo = new FavoritesRepository(mockMapper.Object, Options.Create(new UserErrors()), Options.Create(new DbConnectionOptions()));

//            // Act
//            var result = await repo.GetItemsAsync((Guid)userId);

//            // Assert
//            Assert.True(result.Success);
//            var subscriptionsResult = Assert.IsType<List<CatalogItem>>(result.Value);
//            Assert.NotNull(subscriptionsResult);
//            Assert.Equal(subscriptionsResult.FirstOrDefault()?.AssetId, favoriteEntities.FirstOrDefault()?.AssetId);
//        }

//        [Fact]
//        public async void AddItemAsync_ReturnsSuccessResult()
//        {
//            // Arrange
//            var favoriteEntity = new FavoritesMocks().FavoritesEntities.FirstOrDefault();

//            var mockDbService = new Mock<IDbService>();
//            mockDbService.Setup(db => db.InsertItem<FavoriteEntity>(It.IsAny<FavoriteEntity>())).ReturnsAsync(true);
//            var mockMapper = new Mock<IMapper>();
//            mockMapper.Setup(map => map.Map<CatalogItem, FavoriteEntity> (It.IsAny<CatalogItem>())).Returns(favoriteEntity);

//            var repo = new FavoritesRepository(mockMapper.Object, Options.Create(new UserErrors()), Options.Create(new DbConnectionOptions()));

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
//            var userId = new FavoritesMocks().FavoritesEntities.FirstOrDefault()?.UserId;
//            var favoriteEntities = new FavoritesMocks().FavoritesEntities.Where(a => a.UserId == userId).ToList();
//            var item = new FavoritesMocks().Favorites.FirstOrDefault();

//            var mockDbService = new Mock<IDbService>();
//            mockDbService.Setup(db => db.GetItemsWhere<FavoriteEntity>(nameof(FavoriteEntity.UserId), userId)).ReturnsAsync(favoriteEntities);
//            mockDbService.Setup(db => db.DeleteItem<FavoriteEntity>(It.IsAny<FavoriteEntity>())).ReturnsAsync(true);
//            var mockMapper = new Mock<IMapper>();

//            var repo = new FavoritesRepository(mockMapper.Object, Options.Create(new UserErrors()), Options.Create(new DbConnectionOptions()));

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
//            var userId = new FavoritesMocks().FavoritesEntities.FirstOrDefault()?.UserId;
//            var favoriteEntities = new FavoritesMocks().FavoritesEntities.Where(a => a.UserId == userId).ToList();
//            var item = new FavoritesMocks().Favorites.FirstOrDefault();

//            var mockDbService = new Mock<IDbService>();
//            mockDbService.Setup(db => db.GetItemsWhere<FavoriteEntity>(nameof(FavoriteEntity.UserId), userId)).ReturnsAsync(favoriteEntities);
//            mockDbService.Setup(db => db.UpdateItem<FavoriteEntity>(It.IsAny<FavoriteEntity>())).ReturnsAsync(true);
//            var mockMapper = new Mock<IMapper>();

//            var repo = new FavoritesRepository(mockMapper.Object, Options.Create(new UserErrors()), Options.Create(new DbConnectionOptions()));

//            // Act
//            var result = await repo.UpdateItemAsync((Guid)userId, item);

//            // Assert
//            Assert.True(result.Success);
//            var boolResult = Assert.IsType<bool>(result.Value);
//            Assert.True(boolResult);
//        }
//    }
//}
