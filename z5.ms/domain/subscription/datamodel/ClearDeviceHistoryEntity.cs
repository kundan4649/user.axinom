using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace z5.ms.domain.subscription.datamodel
{
    /// <summary>DB DTO for history of clear devices for users.</summary>
    [Table("ClearDeviceHistory")]
    public class ClearDeviceHistoryEntity
    {
        /// <summary>The human readable name of the device</summary>
        [Key]
        public Guid UserId { get; set; }

        /// <summary>The date and time when the devices cleared</summary>
        public DateTime ClearDate { get; set; }
    }
}
