using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using z5.ms.common.assets.common;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.datamodels
{
    /// <summary>Database entity type for ReminderItem</summary>
    [Table("Reminders")]
    public class ReminderItemEntity
    {
        /// <summary>Unique Id of the reminder item entity</summary>
        [Key]
        public virtual Guid Id { get; set; }

        /// <summary>Unique Id of the user</summary>
        public Guid UserId { get; set; }

        /// <summary>The unique ID of the catalog item.</summary>
        public string AssetId { get; set; }

        /// <summary>Unique asset type</summary>
        public AssetType AssetType { get; set; }

        /// <summary>Remind for upcoming EPG program via</summary>
        public ReminderType ReminderType { get; set; }
    }
}
