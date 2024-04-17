using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace z5.ms.domain.user.datamodels
{
    /// <summary>Database entity type for SettingItem</summary>
    [Table("Settings")]
    public class SettingItemEntity
    {
        /// <summary>Unique Id of the setting item entity</summary>
        [Key]
        public virtual Guid Id { get; set; }

        /// <summary>Unique Id of the user</summary>
        public Guid UserId { get; set; }

        /// <summary>The key of the setting</summary>
        public string SettingKey { get; set; }

        /// <summary>The value of the setting.</summary>
        public string SettingValue { get; set; }

        /// <summary>Last updated date of the setting</summary>
        public DateTime? LastUpdate { get; set; }
    }
}
