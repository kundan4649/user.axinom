using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using z5.ms.common.assets;
using z5.ms.common.assets.common;

namespace z5.ms.user.serverless.azure.reminders
{
    /// <summary>Aggregated model of reminders and epgs</summary>
    public class EpgReminder
    {
        /// <summary>Database entity type for ReminderItem</summary>
        public ReminderItemEntity Reminder { get; set; }

        /// <summary>Database entity type for User</summary>
        public UserEntity User { get; set; }

        /// <summary>Subset of EPG program response object for reminders</summary>
        public EpgProgramReminder Epg { get; set; }
    }

    // TODO: When this moves to user domain, use the entity from there
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

    // TODO: When this moves to user domain, use the entity from there
    /// <summary>Remind for upcoming EPG program via</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ReminderType
    {
        /// <summary>Email</summary>
        [EnumMember(Value = "Email")] Email = 0,

        /// <summary>Mobile</summary>
        [EnumMember(Value = "Mobile")] Mobile = 1
    }
}