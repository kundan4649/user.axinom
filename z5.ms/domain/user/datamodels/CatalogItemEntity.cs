using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using z5.ms.common.assets.common;

namespace z5.ms.domain.user.datamodels
{
    /// <summary>Database entity type for CatalogItem</summary>
    public class CatalogItemEntity
    {
        /// <summary>Unique Id of the catalog item entity</summary>
        public virtual Guid Id { get; set; }

        /// <summary>Unique Id of the user</summary>
        public Guid UserId { get; set; }

        /// <summary>The unique asset ID of the catalog item.</summary>
        public string AssetId { get; set; }

        /// <summary>Unique asset type</summary>
        public AssetType AssetType { get; set; }

        /// <summary>The playback position of this item in seconds if available. Defaults to zero.</summary>
        public int Duration { get; set; }

        /// <summary>Date when the item was added to the list. Defaults to server side UTC "now".</summary>
        public DateTime Date { get; set; }
    }

    /// <summary>Database entity type for Favorites</summary>
    [Table("Favorites")]
    public class FavoriteEntity : CatalogItemEntity
    {
        /// <inheritdoc />
        [Key]
        public override Guid Id { get; set; }

    }

    /// <summary>Database entity type for WatchHistory</summary>
    [Table("WatchHistory")]
    public class WatchHistoryEntity : CatalogItemEntity
    {
        /// <inheritdoc />
        [Key]
        public override Guid Id { get; set; }
    }

    /// <summary>Database entity type for Watchlist</summary>
    [Table("Watchlist")]
    public class WatchlistEntity : CatalogItemEntity
    {
        /// <inheritdoc />
        [Key]
        public override Guid Id { get; set; }
    }
}
