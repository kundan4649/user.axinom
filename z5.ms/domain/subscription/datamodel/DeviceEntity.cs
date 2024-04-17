using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace z5.ms.domain.subscription.datamodel
{
    /// <summary>DB DTO for a device.</summary>
    [Table("Devices")]
    public class DeviceEntity
    {
        /// <summary>The unique database ID of the device</summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>The human readable name of the device</summary>
        public Guid UserId { get; set; }

        /// <summary>The unique identifier of the device - created by the client.</summary>
        public string Identifier { get; set; }

        /// <summary>The human readable name of the device</summary>
        public string Name { get; set; }

        /// <summary>The date and time when the device was added</summary>
        public DateTime CreateDate { get; set; }
    }
}
