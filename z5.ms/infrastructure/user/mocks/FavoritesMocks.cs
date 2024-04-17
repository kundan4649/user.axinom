using System;
using System.Collections.Generic;
using z5.ms.common.assets.common;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.mocks
{
    /// <summary>
    /// Mock data for testing Favorites endpoints
    /// </summary>
    public class FavoritesMocks
    {
        /// <summary>Mock favorites data</summary>
        public List<CatalogItem> Favorites = new List<CatalogItem>
        {
            new CatalogItem
            {
                AssetId = "0-0-Transformers",
                AssetType = AssetType.Movie,
                Date = DateTime.Parse("2017-04-20T17:43:31.27Z")
            },
            new CatalogItem
            {
                AssetId = "0-0-Logan",
                AssetType = AssetType.Movie,
                Date = DateTime.Parse("2017-01-20T17:43:31.27Z")
            },
            new CatalogItem
            {
                AssetId = "0-0-GhostInTheShell",
                AssetType = AssetType.Movie,
                Date = DateTime.Parse("2017-07-20T17:43:31.27Z")
            },
            new CatalogItem
            {
                AssetId = "0-0-Dangal",
                AssetType = AssetType.Movie,
                Date = DateTime.Parse("2017-09-20T17:43:31.27Z")
            }
        };

        /// <summary>Mock favorite entity data</summary>
        public List<FavoriteEntity> FavoritesEntities = new List<FavoriteEntity>
        {
            new FavoriteEntity
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("4ad1877e-3cb3-4dfb-b5d1-9faebff2bf29"),
                AssetId = "0-0-Transformers",
                AssetType = AssetType.Movie,
                Date = DateTime.Parse("2017-04-20T17:43:31.27Z")
            },
            new FavoriteEntity
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("4ad1877e-3cb3-4dfb-b5d1-9faebff2bf29"),
                AssetId = "0-0-Logan",
                AssetType = AssetType.Movie,
                Date = DateTime.Parse("2017-01-20T17:43:31.27Z")
            },
            new FavoriteEntity
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("4ad1877e-3cb3-4dfb-b5d1-9faebff2bf29"),
                AssetId = "0-0-GhostInTheShell",
                AssetType = AssetType.Movie,
                Date = DateTime.Parse("2017-07-20T17:43:31.27Z")
            },
            new FavoriteEntity
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("4ad1877e-3cb3-4dfb-b5d1-9faebff2bf29"),
                AssetId = "0-0-Dangal",
                AssetType = AssetType.Movie,
                Date = DateTime.Parse("2017-09-20T17:43:31.27Z")
            }
        };
    }
}
