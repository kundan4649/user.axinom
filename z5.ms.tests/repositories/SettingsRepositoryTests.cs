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
//    public class SettingsRepositoryTests
//    {
//        [Fact]
//        public async void GetItemsAsync_ReturnsSettings()
//        {
//            // Arrange
//            var userId = new SettingsMocks().SettingsEntities.FirstOrDefault()?.UserId;
//            var settingsEntities = new SettingsMocks().SettingsEntities.Where(a => a.UserId == userId).ToList();

//            var mockDbService = new Mock<IDbService>();
//            mockDbService.Setup(db => db.GetItemsWhere<SettingItemEntity>(nameof(SettingItemEntity.UserId), userId)).ReturnsAsync(settingsEntities);
//            var mockMapper = new Mock<IMapper>();
//            mockMapper.Setup(map => map.Map<SettingItemEntity, SettingItem>(It.IsAny<SettingItemEntity>())).Returns(new SettingsMocks().Settings.FirstOrDefault());

//            var repo = new SettingsRepository(mockMapper.Object, Options.Create(new UserErrors()), Options.Create(new DbConnectionOptions()));

//            // Act
//            var result = await repo.GetItemsAsync((Guid)userId);

//            // Assert
//            Assert.True(result.Success);
//            var subscriptionsResult = Assert.IsType<List<SettingItem>>(result.Value);
//            Assert.NotNull(subscriptionsResult);
//            Assert.Equal(subscriptionsResult.FirstOrDefault()?.SettingKey, settingsEntities.FirstOrDefault()?.SettingKey);
//        }

//        [Fact]
//        public async void AddItemAsync_ReturnsSuccessResult()
//        {
//            // Arrange
//            var settingsEntity = new SettingsMocks().SettingsEntities.FirstOrDefault();

//            var mockDbService = new Mock<IDbService>();
//            mockDbService.Setup(db => db.InsertItem<SettingItemEntity>(It.IsAny<SettingItemEntity>())).ReturnsAsync(true);
//            var mockMapper = new Mock<IMapper>();
//            mockMapper.Setup(map => map.Map<SettingItem, SettingItemEntity> (It.IsAny<SettingItem>())).Returns(settingsEntity);

//            var repo = new SettingsRepository(mockMapper.Object, Options.Create(new UserErrors()), Options.Create(new DbConnectionOptions()));

//            // Act
//            var result = await repo.AddItemAsync(Guid.NewGuid(), new SettingItem());

//            // Assert
//            Assert.True(result.Success);
//            var boolResult = Assert.IsType<bool>(result.Value);
//            Assert.True(boolResult);
//        }

//        [Fact]
//        public async void DeleteItemAsync_ReturnsSuccessResult()
//        {
//            // Arrange
//            var userId = new SettingsMocks().SettingsEntities.FirstOrDefault()?.UserId;
//            var settingsEntities = new SettingsMocks().SettingsEntities.Where(a => a.UserId == userId).ToList();
//            var item = new SettingsMocks().Settings.FirstOrDefault();

//            var mockDbService = new Mock<IDbService>();
//            mockDbService.Setup(db => db.GetItemsWhere<SettingItemEntity>(nameof(SettingItemEntity.UserId), userId)).ReturnsAsync(settingsEntities);
//            mockDbService.Setup(db => db.DeleteItem<SettingItemEntity>(It.IsAny<SettingItemEntity>())).ReturnsAsync(true);
//            var mockMapper = new Mock<IMapper>();

//            var repo = new SettingsRepository(mockMapper.Object, Options.Create(new UserErrors()), Options.Create(new DbConnectionOptions()));

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
//            var userId = new SettingsMocks().SettingsEntities.FirstOrDefault()?.UserId;
//            var settingsEntities = new SettingsMocks().SettingsEntities.Where(a => a.UserId == userId).ToList();
//            var item = new SettingsMocks().Settings.FirstOrDefault();

//            var mockDbService = new Mock<IDbService>();
//            mockDbService.Setup(db => db.GetItemsWhere<SettingItemEntity>(nameof(SettingItemEntity.UserId), userId)).ReturnsAsync(settingsEntities);
//            mockDbService.Setup(db => db.UpdateItem<SettingItemEntity>(It.IsAny<SettingItemEntity>())).ReturnsAsync(true);
//            var mockMapper = new Mock<IMapper>();

//            var repo = new SettingsRepository(mockMapper.Object, Options.Create(new UserErrors()), Options.Create(new DbConnectionOptions()));

//            // Act
//            var result = await repo.UpdateItemAsync((Guid)userId, item);

//            // Assert
//            Assert.True(result.Success);
//            var boolResult = Assert.IsType<bool>(result.Value);
//            Assert.True(boolResult);
//        }
//    }
//}
