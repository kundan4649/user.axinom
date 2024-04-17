using System;
using System.Collections.Generic;
using z5.ms.common.assets.common;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.mocks
{
    /// <summary>
    /// Mock data for testing WatchHistory endpoints
    /// </summary>
    public class WatchHistoryMocks
    {
        /// <summary>Mock watch history data</summary>
        public List<CatalogItem> WatchHistory = new List<CatalogItem>
        {
            new CatalogItem
            {
                AssetId = "0-0-TheMummy",
                AssetType = AssetType.Movie,
                Duration = 85,
                Date = DateTime.Parse("2017-04-20T17:43:31.27Z")
            },
            new CatalogItem
            {
                AssetId = "0-0-TheWall",
                AssetType = AssetType.Movie,
                Duration = 95,
                Date = DateTime.Parse("2017-01-20T17:43:31.27Z")
            },
            new CatalogItem
            {
                AssetId = "0-0-Dunkirk",
                AssetType = AssetType.Movie,
                Duration = 91,
                Date = DateTime.Parse("2017-07-20T17:43:31.27Z")
            },
            new CatalogItem
            {
                AssetId = "0-0-TheFateOfTheFurious",
                AssetType = AssetType.Movie,
                Duration = 135,
                Date = DateTime.Parse("2017-09-20T17:43:31.27Z")
            }
        };

        /// <summary>Mock watch history entity data</summary>
        public List<WatchHistoryEntity> WatchHistoryEntities = new List<WatchHistoryEntity>
        {
            new WatchHistoryEntity
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("4ad1877e-3cb3-4dfb-b5d1-9faebff2bf29"),
                AssetId = "0-0-TheMummy",
                AssetType = AssetType.Movie,
                Duration = 85,
                Date = DateTime.Parse("2017-04-20T17:43:31.27Z")
            },
            new WatchHistoryEntity
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("4ad1877e-3cb3-4dfb-b5d1-9faebff2bf29"),
                AssetId = "0-0-TheWall",
                AssetType = AssetType.Movie,
                Duration = 95,
                Date = DateTime.Parse("2017-01-20T17:43:31.27Z")
            },
            new WatchHistoryEntity
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("4ad1877e-3cb3-4dfb-b5d1-9faebff2bf29"),
                AssetId = "0-0-Dunkirk",
                AssetType = AssetType.Movie,
                Duration = 91,
                Date = DateTime.Parse("2017-07-20T17:43:31.27Z")
            },
            new WatchHistoryEntity
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("4ad1877e-3cb3-4dfb-b5d1-9faebff2bf29"),
                AssetId = "0-0-TheFateOfTheFurious",
                AssetType = AssetType.Movie,
                Duration = 135,
                Date = DateTime.Parse("2017-09-20T17:43:31.27Z")
            }
        };
    }
}
